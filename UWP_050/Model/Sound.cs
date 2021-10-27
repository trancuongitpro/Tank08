using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace UWP_050.Model
{
    public class Sound
    {
        public string Name { get; set; }
        public SoundCategory Category { get; set; }
        public string AudioFile { get; set; }
        public string ImageFile { get; set; }

        public Sound(string name, SoundCategory category)
        {
            Name = name;
            Category = category;
            AudioFile = string.Format("/Assets/Audio/{0}/{1}.mp3", category, name);
            ImageFile = string.Format("/Assets/Images/{0}/{1}.png", category, name);
        }
    }
    public enum SoundCategory
    {
        Animals,
        Cartoons,
        Taunts,
        Warnings
    }

    public class SoundManager
    {

        private static List<Sound> GetSounds()
        {

            var sounds = new List<Sound>();
            sounds.Add(new Sound("Cow", SoundCategory.Animals));
            sounds.Add(new Sound("Cat", SoundCategory.Animals));
            sounds.Add(new Sound("duongvecoem", SoundCategory.Animals));

            sounds.Add(new Sound("Gun", SoundCategory.Cartoons));
            sounds.Add(new Sound("Spring", SoundCategory.Cartoons));

            sounds.Add(new Sound("Clock", SoundCategory.Taunts));
            sounds.Add(new Sound("LOL", SoundCategory.Taunts));

            sounds.Add(new Sound("Ship", SoundCategory.Warnings));
            sounds.Add(new Sound("Siren", SoundCategory.Warnings));

            return sounds;
        }

        public static void getAllSounds(ObservableCollection<Sound> sounds)
        {
            var AllSounds = GetSounds();
            sounds.Clear();
            AllSounds.ForEach(p => sounds.Add(p));
        }
        public static void GetSoundsByCategory(ObservableCollection<Sound> sounds, SoundCategory soundcategory)
        {
            var allSouds = GetSounds();
            var filteredSounds = allSouds.Where(p => p.Category == soundcategory).ToList();
            sounds.Clear();
            filteredSounds.ForEach(p => sounds.Add(p));
        }

        public static void getSoundsByName(ObservableCollection<Sound> sounds, string name)
        {
            var allSounds = GetSounds();
            var fillteredSounds = allSounds.Where(p => p.Name == name).ToList();
            sounds.Clear();
            fillteredSounds.ForEach(p => sounds.Add(p));
        }
    }

}
