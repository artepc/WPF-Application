using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Solution
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// to allow to list all items, add a new one item, edit and delete it
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionViewSource personViewSource;
        DataContext context;
        Person newItem = new Person();
        Person chosenItem = new Person();

        public MainWindow(DataContext context)
        {
            this.context = context;
            InitializeComponent();
            personViewSource = (CollectionViewSource)FindResource(nameof(personViewSource));
            GetItemslist();
            ItemGrid.DataContext = newItem;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // load the entities into EF Core
            context.People.Include(k=>k.Phones).Load();

            // bind to the source
            personViewSource.Source =
                context.People.Local.ToObservableCollection();
        }

        // Get list of all items
        private void GetItemslist()
        {
            PeopleView.ItemsSource = context.People
                .Include(k => k.Phones)     
                .ToList();
        }

        private void AddNewItem(object sender, RoutedEventArgs e)
        {
           Person chosenPerson = PeopleView.SelectedItem as Person;
         
            if (newItem.PersonId == chosenPerson?.PersonId )
            {
                MessageBox.Show("This item is allready in database. If you want to edit this item, press button edit");
                return;

            }
            context.Add(newItem);        
            context.SaveChanges();
            GetItemslist();
            newItem = new Person();
            ItemGrid.DataContext = newItem;
        }

        private void ClearItemForm(object sender, RoutedEventArgs e)
        {
            newItem = new Person();
            ItemGrid.DataContext = newItem;
        }

        private void UpdateItem(object sender, RoutedEventArgs e)
        {

            // firstly the item is added without the phone numbers. These numbers are added first if the item is edited. 
            foreach (var phone in context.Phones.Local.Where(p => p.PersonId == chosenItem.PersonId).ToList())
            {
                chosenItem.Phones.Add(phone);
            }

            context.Update(chosenItem);
            context.SaveChanges();
            GetItemslist();
            newItem = new Person();
            ItemGrid.DataContext = newItem;
        }

        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            context.People.Remove(chosenItem);
            context.SaveChanges();
            GetItemslist();
            newItem = new Person();
            ItemGrid.DataContext = newItem;
        }

        private void ShowSearchedItem(object sender, RoutedEventArgs e)
        {
            var text = sender as TextBox;
            if (text.Text != "")
            {
                var searchedList = context.People.Where(value => value.LastName.ToLower().Contains(text.Text.ToLower())
                 || value.FirstName.ToLower().Contains(text.Text.ToLower())
                 || value.City.ToLower().Contains(text.Text.ToLower())
                 || value.Street.ToLower().Contains(text.Text.ToLower())
                 || value.ZipCode.ToString().ToLower().Contains(text.Text.ToLower())).ToList();
                PeopleView.ItemsSource = null;
                PeopleView.ItemsSource = searchedList;
            }
            else
            {
                GetItemslist();
            }
        }

        // click to choose one item to update or delete it
        private void ClickToChooseItem(object sender, SelectionChangedEventArgs e)
        {
            chosenItem = PeopleView.SelectedItem as Person;

            if (chosenItem != null)
            {
                ItemGrid.DataContext = chosenItem;

            }
        }

        // shows the phones based on PersonId
        private void ShowPhoneInformation(object sender, RoutedEventArgs e)
        {

            Person chosenItem = PeopleView.SelectedItem as Person;
            if (chosenItem != null)
            {
                Person person = context.People.Where(p => p.PersonId == chosenItem.PersonId).First();
              
                phoneDataGrid.ItemsSource = context.Phones
               .Include(k => k.Person)
               .Where(k => k.PersonId == person.PersonId)
               .ToList();
         
            }
        }

        private void UploadPhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Select a picture";
            openDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (openDialog.ShowDialog() == true)
            {
                imagePhoto.Source = new BitmapImage(new Uri(openDialog.FileName));
                
            }
        }
    }
}
