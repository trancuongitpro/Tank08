using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_050.Model;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_050
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Sound> Sounds;

        private List<MenuItem> MenuItems;

        private List<string> Suggestions;
        public MainPage()
        {
            this.InitializeComponent();
            Sounds = new ObservableCollection<Sound>();
            SoundManager.getAllSounds(Sounds);
            MenuItems = new List<MenuItem>();
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icons/animals.png", Category = SoundCategory.Animals });
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icons/cartoon.png", Category = SoundCategory.Cartoons });
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icons/taunt.png", Category = SoundCategory.Taunts });
            MenuItems.Add(new MenuItem { IconFile = "Assets/Icons/warning.png", Category = SoundCategory.Warnings });
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySpilitView.IsPaneOpen = !MySpilitView.IsPaneOpen;

        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.getAllSounds(Sounds);
            CategoryTextBlock.Text = "all Sounds";
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Collapsed;
            goBack();
        }

        private void goBack()
        {
            SoundManager.getAllSounds(Sounds);
            CategoryTextBlock.Text = "all sounds";
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SoundManager.getSoundsByName(Sounds, sender.Text);
            CategoryTextBlock.Text = sender.Text;
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Visible;
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text)) goBack();
            SoundManager.getAllSounds(Sounds);
            Suggestions = Sounds.Where(p => p.Name.StartsWith(sender.Text)).Select(p => p.Name).ToList();
            SearchAutoSuggestBox.ItemsSource = Suggestions;
        }
        private void MenuItemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = (MenuItem)e.ClickedItem;
            CategoryTextBlock.Text = menuItem.Category.ToString();
            SoundManager.GetSoundsByCategory(Sounds, menuItem.Category);
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void SounlGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var sound = (Sound)e.ClickedItem;
            MyMediaElement.Source = new Uri(this.BaseUri, sound.AudioFile);
        }

        private void MenuItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void SounlGridView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems)) ;
            {
                var item = await e.DataView.GetStorageItemsAsync();

                if (item.Any())
                {
                    var storageFile = item[0] as StorageFile;
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;

                    if (contentType == "audio/wabv" || contentType == "audio/mpeg")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(
                            folder,
                            storageFile.Name,
                            NameCollisionOption.GenerateUniqueName);
                        MyMediaElement.SetSource(
                            await storageFile.OpenAsync(FileAccessMode.Read),
                            contentType);
                        MyMediaElement.Play();
                    }
                }
            }
        }

        private void SounlGridView_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drop to create a custom sound and title";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }


    }

}
