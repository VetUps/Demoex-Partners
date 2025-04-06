using Microsoft.EntityFrameworkCore;
using some_products_app2.Models;
using some_products_app2.Views;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace some_products_app2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetFilterSource();
            SetSortSource();

            DataContext = this;
            LoadPartnersAsync();
        }

        // Измените тип коллекции на ObservableCollection
        private List<ProductCard> _productCards = new();
        public List<ProductCard> ProductCardSource
        {
            get => _productCards;
            set
            {
                _productCards = value;
            }
        }

        public async Task LoadPartnersAsync()
        {
            try
            {
                using var db = new AppDbContext();

                var partners = await db.PartnerImports.Include(o => o.Director).
                                                 Include(o => o.PartnerType)
                                                .Include(o => o.Sales).AsNoTracking().ToListAsync();

                ProductCardSource.Clear();

                foreach (var partner in partners)
                {
                    ProductCardSource.Add(new ProductCard(partner));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void SetFilterSource()
        {
            using var db = new AppDbContext();
            List<string> filters = db.PartnerTypes.Select(pt => pt.PartnerTypeName).ToList();
            filters.Add("Без фильтрации");

            filterComboBox.ItemsSource = filters;
        }

        private void SortPartners(string sortMethod)
        {
            ProductCardSource = ProductCardSource.OrderBy(o => o.Partner.CalculateDiscount).ToList();
            partnersListView.ItemsSource = null;

            if (sortMethod.Last() == '↓')
            {
                // Что-то может быть
            }
            else if (sortMethod.Last() == '↑')
            {
                ProductCardSource.Reverse();
            }

            partnersListView.ItemsSource = ProductCardSource;
        }

        private void FilterPartners(string filterMethod)
        {
            if (filterMethod == "Без фильтрации")
            {
                LoadPartnersAsync();
                partnersListView.ItemsSource = null;
                partnersListView.ItemsSource = ProductCardSource;
            }

            else
            {
                List<ProductCard> filteredCardSource = ProductCardSource.Where(o => o.Partner.PartnerType.PartnerTypeName == filterMethod).ToList();
                partnersListView.ItemsSource = null;
                partnersListView.ItemsSource = filteredCardSource;
            }
        }

        private void SearchPartners(string serachField)
        {
            if (string.IsNullOrWhiteSpace(serachField) || string.IsNullOrEmpty(serachField))
            {
                partnersListView.ItemsSource = null;
                partnersListView.ItemsSource = ProductCardSource;
            }

            else
            {
                List<ProductCard> filteredCardSource = ProductCardSource.Where(o => o.Partner.PartnerImportName.Contains(serachField)).ToList();
                partnersListView.ItemsSource = null;
                partnersListView.ItemsSource = filteredCardSource;
            }
        }

        private void SetSortSource()
        {
            sortComboBox.ItemsSource = new List<string> { "По рейтингу ↓", "По рейтингу ↑" };
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            ProductCard card = partnersListView.SelectedItem as ProductCard;
            ProductCardSource.Remove(card);

            partnersListView.ItemsSource = null;
            partnersListView.ItemsSource = ProductCardSource;

            using (var db = new AppDbContext())
            {
                db.PartnerImports.Remove(card.Partner);
                db.SaveChanges();
            }
        }

        private void RedactItem(object sender, RoutedEventArgs e)
        {
            ProductCard card = partnersListView.SelectedItem as ProductCard;
            new PartnerAddRedactWindow(card.Partner).Show();
            Close();
        }

        private void addPartnerButton_Click(object sender, RoutedEventArgs e)
        {
            new PartnerAddRedactWindow(null).Show();
            Close();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortPartners(sortComboBox.SelectedItem.ToString());
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPartners(filterComboBox.SelectedItem.ToString());
        }

        private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            SearchPartners(searchTextBox.Text.ToLower());
        }
    }
}