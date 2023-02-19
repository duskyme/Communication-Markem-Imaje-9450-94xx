using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Windows;

public class ProfileManager
{
   

    private List<Profile> _profiles;
    private string _filePath;

    public ProfileManager(string filePath)
    {
        _filePath = filePath;
        _profiles = new List<Profile>();
    }

    public void LoadProfiles()
    {
        
            if (!File.Exists(_filePath))
            {
            _profiles = new List<Profile>
{
    new Profile("Profil1", 1, 3, 3, 10, 1, 2, 0, 1, 1, 0, 1, 1, 200, 0),
    new Profile("Profil2", 2, 4, 4, 2, 1, 3, 1, 1, 1, 1, 1, 1, 300, 0),
    new Profile("Profil3", 3, 5, 5, 3, 2, 4, 2, 0, 0, 0, 0, 0, 400, 0),
    new Profile("Profil4", 4, 6, 6, 4, 3, 5, 3, 1, 1, 1, 1, 1, 500, 0)
};
            SaveProfiles();
                MessageBox.Show("Boli vytvorené nové náhodné profily.");
                return;
            }

            var serializer = new XmlSerializer(typeof(List<Profile>));
            using (var reader = new StreamReader(_filePath))
            {
                _profiles = (List<Profile>)serializer.Deserialize(reader);
            }

            foreach (var profile1 in _profiles)
            {
                if (string.IsNullOrEmpty(profile1.Name))
                {
                    MessageBox.Show("Chyba: Pole Name pre profil " + profile1.Name + " nie je vyplnené");
                }

                if (profile1.FontSizePro < 0 || profile1.FontSizePro > 5)
                {
                    MessageBox.Show("Chyba: Pole FontSizeProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.MedzerapredPro < 3 || profile1.MedzerapredPro > 6363)
                {
                    MessageBox.Show("Chyba: Pole MedzerapredProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.MedzerazaPro < 3 || profile1.MedzerazaPro > 6363)
                {
                    MessageBox.Show("Chyba: Pole MedzerazaProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.DelenietachaPro < 1 || profile1.DelenietachaPro > 63)
                {
                    MessageBox.Show("Chyba: Pole DelenietachaProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.PoceopPro < 0 || profile1.PoceopPro > 255)
                {
                    MessageBox.Show("Chyba: Pole PoceopProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.IntervapopPro < 2 || profile1.IntervapopPro > 6363)
                {
                    MessageBox.Show("Chyba: Pole IntervapopProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.ExpansePro < 0 || profile1.ExpansePro > 3)
                {
                    MessageBox.Show("Chyba: Pole ExpanseProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.JobdirectionPro < 0 || profile1.JobdirectionPro > 1)
                {
                    MessageBox.Show("Chyba: Pole JobdirectionProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.CharVdirectPro < 0 || profile1.CharVdirectPro > 1)
                {
                    MessageBox.Show("Chyba: Pole CharVdirectProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }
                if (profile1.CharHdirectPro < 0 || profile1.CharHdirectPro > 1)
                {
                    MessageBox.Show("Chyba: Pole CharHdirectProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.PrintmodePro < 0 || profile1.PrintmodePro > 1)
                {
                    MessageBox.Show("Chyba: Pole PrintmodeProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.SpeedmodePro < 0 || profile1.SpeedmodePro > 1)
                {
                    MessageBox.Show("Chyba: Pole SpeedmodeProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.PruductSpeedPro < 2 || profile1.PruductSpeedPro > 5745)
                {
                    MessageBox.Show("Chyba: Pole PruductSpeedProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

                if (profile1.UnitsPro < 0 || profile1.UnitsPro > 1)
                {
                    MessageBox.Show("Chyba: Pole UnitsProdatapro pre profil " + profile1.Name + " má neplatnú hodnotu");
                }

            }
        
    }

    public void SaveProfiles()
    {
        var serializer = new XmlSerializer(typeof(List<Profile>));
        using (var writer = new StreamWriter(_filePath))
        {
            serializer.Serialize(writer, _profiles);
        }
    }

    public List<Profile> Profiles
    {
        get { return _profiles; }
    }

    public void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
    }

    public List<Profile> GetProfiles()
    {
        return _profiles;
    }
}

[XmlRoot("Profile")]
public class Profile
{
    [XmlElement("Name")]
    public string Name { get; set; }
    [XmlElement("FontProIndex")]
    public int FontSizePro { get; set; }
    [XmlElement("MedzerapredPro")]
    public int MedzerapredPro { get; set; }
    [XmlElement("MedzerazaPro")]
    public int MedzerazaPro { get; set; }
    [XmlElement("DelenietachaPro")]
    public int DelenietachaPro { get; set; }
    [XmlElement("PoceopPro")]
    public int PoceopPro { get; set; }
    [XmlElement("IntervapopPro")]
    public int IntervapopPro { get; set; }
    [XmlElement("ExpanseProIndex")]
    public int ExpansePro { get; set; }
    [XmlElement("JobdirectionProIndex")]
    public int JobdirectionPro { get; set; }
    [XmlElement("CharVdirectProIndex")]
    public int CharVdirectPro { get; set; }
    [XmlElement("CharHdirectProIndex")]
    public int CharHdirectPro { get; set; }
    [XmlElement("PrintmodeProIndex")]
    public int PrintmodePro { get; set; }
    [XmlElement("SpeedmodeProIndex")]
    public int SpeedmodePro { get; set; }
    [XmlElement("PruductSpeedPro")]
    public int PruductSpeedPro { get; set; }
    [XmlElement("UnitsProIndex")]
    public int UnitsPro { get; set; }

    public Profile()
    {
        // Default constructor for serialization purposes
    }

    public Profile(string name,int fontSizePro, int medzerapredPro, int medzerazaPro, int delenietachaPro, int poceopPro, int intervapopPro, int expansePro, int jobdirectionPro, int charVdirectPro, int charHdirectPro, int printmodePro, int speedmodePro, int pruductSpeedPro, int unitsPro)
    {
        Name= name;
        FontSizePro = fontSizePro;
        MedzerapredPro = medzerapredPro;
        MedzerazaPro = medzerazaPro;
        DelenietachaPro = delenietachaPro;
        PoceopPro = poceopPro;
        IntervapopPro = intervapopPro;
        ExpansePro = expansePro;
        JobdirectionPro = jobdirectionPro;
        CharVdirectPro = charVdirectPro;
        CharHdirectPro = charHdirectPro;
        PrintmodePro = printmodePro;
        SpeedmodePro = speedmodePro;
        PruductSpeedPro = pruductSpeedPro;
        UnitsPro = unitsPro;
    }
}