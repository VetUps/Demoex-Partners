using some_products_app2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace some_products_app2.Views
{
    /// <summary>
    /// Логика взаимодействия для PartnerAddRedactWindow.xaml
    /// </summary>
    public partial class PartnerAddRedactWindow : Window
    {
        public PartnerAddRedactWindow(PartnerImport? partnerForRedact)
        {
            InitializeComponent();
            partnerTypeComboBox.ItemsSource = GetPartnerTypes().Select(o => o.PartnerTypeName);
            PartnerForRedact = partnerForRedact;
            if (partnerForRedact != null) 
            {
                FillTextEdits();
            }

            SetValidationRules();
        }

        public PartnerImport? PartnerForRedact { get; set; }
        private readonly Dictionary<TextBox, List<(Func<string, bool> rule, string error)>> _validationRules = [];

        private void AddValidationRule(TextBox textBox, Func<string, bool> rule, string error)
        {
            if (!_validationRules.ContainsKey(textBox))
            {
                _validationRules[textBox] = new List<(Func<string, bool>, string)>();
            }
            _validationRules[textBox].Add((rule, error));
        }

        private void SetValidationRules()
        {
            AddValidationRule(partnerNameTextBox, s => !string.IsNullOrWhiteSpace(s), "Имя партнёра обязательно!");
            AddValidationRule(directorSurnameTextBox, s => !string.IsNullOrWhiteSpace(s), "Фамилия директора обязательна!");
            AddValidationRule(directorNameTextBox, s => !string.IsNullOrWhiteSpace(s), "Имя директора обязательно!");
            AddValidationRule(directorPatronymicTextBox, s => !string.IsNullOrWhiteSpace(s), "Отчество директора обязательно!");
            AddValidationRule(directorEmailTextBox, s => !string.IsNullOrWhiteSpace(s), "Электронная почта директора обязательна!");
            AddValidationRule(directorPhoneTextBox, s => !string.IsNullOrWhiteSpace(s), "Телефон директора обязателен!");
            AddValidationRule(partnerIndexTextBox, s => !string.IsNullOrWhiteSpace(s), "Индекс партнёра обязателен!");
            AddValidationRule(partnerAreaTextBox, s => !string.IsNullOrWhiteSpace(s), "Область партнёра обязательна!");
            AddValidationRule(partnerCityTextBox, s => !string.IsNullOrWhiteSpace(s), "Город партнёра обязателен!");
            AddValidationRule(partnerStreetTextBox, s => !string.IsNullOrWhiteSpace(s), "Улица партнёра обязательна!");
            AddValidationRule(partnerHouseTextBox, s => !string.IsNullOrWhiteSpace(s), "Дом партнёра обязателен!");
            AddValidationRule(partnerINNTextBox, s => !string.IsNullOrWhiteSpace(s), "ИНН партнёра обязателен!");
            AddValidationRule(partnerRatingTextBox, s => !string.IsNullOrWhiteSpace(s), "Рейтинг партнёра обязателен!");
            AddValidationRule(partnerRatingTextBox, s => (Convert.ToInt32(s) > 0), "Рейтинг партнёра должен быть положительным!");
        }

        private void ValidateAll()
        {
            foreach (var (textBox, rule_errors) in _validationRules)
            {
                foreach (var (rule, error) in rule_errors)
                {
                    if (!rule(textBox.Text))
                    {
                        throw new Exception(error);
                    }
                }
            }
        }

        private void FillTextEdits()
        {
            Director director = PartnerForRedact.Director;

            directorSurnameTextBox.Text = director.DirectorSurname;
            directorNameTextBox.Text = director.DirectorName;
            directorPatronymicTextBox.Text = director.DirectorPatronymic;
            directorEmailTextBox.Text = director.DirectorEmail;
            directorPhoneTextBox.Text = director.DirectorPhone;

            partnerTypeComboBox.SelectedItem = PartnerForRedact.PartnerType.PartnerTypeName;
            partnerNameTextBox.Text = PartnerForRedact.PartnerImportName;
            partnerIndexTextBox.Text = PartnerForRedact.PartnerImportIndex;
            partnerAreaTextBox.Text = PartnerForRedact.PartnerImportOblast;
            partnerCityTextBox.Text = PartnerForRedact.PartnerImportCity;
            partnerStreetTextBox.Text = PartnerForRedact.PartnerImportStreet;
            partnerHouseTextBox.Text = PartnerForRedact.PartnerImportHouse;
            partnerINNTextBox.Text = PartnerForRedact.PartnerImport1;
            partnerRatingTextBox.Text = PartnerForRedact.PartnerImportRating.ToString();
        }

        private List<PartnerType> GetPartnerTypes()
        {
            using (var db = new AppDbContext())
            {
                List<PartnerType> partnerTypes = db.PartnerTypes.ToList();
                return partnerTypes;
            }
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void savePartnerButton_Click(object sender, RoutedEventArgs e)
        {
                try
                {
                    ValidateAll();

                    using var db = new AppDbContext();
                    using var transaction = db.Database.BeginTransaction();

                    Director director = new Director();

                    director.DirectorSurname = directorSurnameTextBox.Text;
                    director.DirectorName = directorNameTextBox.Text;
                    director.DirectorPatronymic = directorPatronymicTextBox.Text;
                    director.DirectorEmail = directorEmailTextBox.Text;
                    director.DirectorPhone = directorPhoneTextBox.Text;

                    if (PartnerForRedact != null)
                    {
                        director.DirectorId = PartnerForRedact.Director.DirectorId;
                        db.Update(PartnerForRedact.Director).CurrentValues.SetValues(director);
                        /*
                        int? oldDirectorId = PartnerForRedact.DirectorId;
                        db.Directors.Remove(PartnerForRedact.Director);

                        director.DirectorId = oldDirectorId.Value;
                        db.Directors.Add(director);
                        */
                    }

                    else
                    {
                        db.Directors.Add(director);
                        db.SaveChanges();

                        director = db.Directors.ToList().Last();
                    }

                    db.SaveChanges();

                    PartnerImport partner = new PartnerImport();

                    List<PartnerType> partnerTypes = db.PartnerTypes.ToList();
                    string selectedPartnerType = partnerTypeComboBox.SelectedItem.ToString();
                    PartnerType partnerType = partnerTypes.Find(o => o.PartnerTypeName == selectedPartnerType);

                    partner.PartnerType = partnerType;
                    partner.PartnerImportName = partnerNameTextBox.Text;
                    partner.Director = director;
                    partner.PartnerImportIndex = partnerIndexTextBox.Text;
                    partner.PartnerImportOblast = partnerAreaTextBox.Text;
                    partner.PartnerImportCity = partnerCityTextBox.Text;
                    partner.PartnerImportStreet = partnerStreetTextBox.Text;
                    partner.PartnerImportHouse = partnerHouseTextBox.Text;
                    partner.PartnerImport1 = partnerINNTextBox.Text;
                    partner.PartnerImportRating = (float?)Convert.ToDouble(partnerRatingTextBox.Text);

                    if (PartnerForRedact != null)
                    {
                        partner.PartnerImportId = PartnerForRedact.PartnerImportId;
                        partner.PartnerTypeId = partnerType.PartnerTypeId;
                        partner.DirectorId = director.DirectorId;
                        db.Update(PartnerForRedact).CurrentValues.SetValues(partner);

                        /*
                        int? oldPartnerId = PartnerForRedact.PartnerImportId;
                        db.PartnerImports.Remove(PartnerForRedact);

                        partner.PartnerImportId = oldPartnerId.Value;
                        db.PartnerImports.Add(PartnerForRedact);
                        */
                    }
                    else
                    {
                        db.PartnerImports.Add(partner);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    MessageBox.Show("Сохранение произошло успешно", "Сохранено", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }
    }
}
