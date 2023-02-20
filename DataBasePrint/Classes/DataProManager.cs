using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Controls;
using System.Windows;

public class DataProManager
{


    private List<Profiledatapro> _profilesdatapro;
    private string _filePathdatapro;

    public DataProManager(string filePathdatapro)
    {
        _filePathdatapro = filePathdatapro;
        _profilesdatapro = new List<Profiledatapro>();
    }

    public void LoadProfilesdatapro()
    {
        if (!File.Exists(_filePathdatapro))
        {
            _profilesdatapro = new List<Profiledatapro>
        {
            new Profiledatapro("Profil1", 1, 3, 35, 14, 0, 200, 1, 1, 1, 0, 1, 1, 1700, 0),
            new Profiledatapro("Profil2", 2, 4, 4, 2, 1, 3, 1, 1, 1, 1, 1, 1, 1700, 0),
            new Profiledatapro("Profil3", 3, 5, 5, 3, 2, 4, 2, 0, 0, 0, 0, 0, 1700, 0),
            new Profiledatapro("Profil4", 4, 6, 6, 4, 3, 5, 3, 1, 1, 1, 1, 1, 1700, 0)
        };
            SaveProfilesdatapro();
            MessageBox.Show("Boli vytvorené nové náhodné profily");
            return;
        }

        var serializer = new XmlSerializer(typeof(List<Profiledatapro>));
        using (var reader = new StreamReader(_filePathdatapro))
        {
            _profilesdatapro = (List<Profiledatapro>)serializer.Deserialize(reader);
        }

        foreach (var profile in _profilesdatapro)
        {
            if (string.IsNullOrEmpty(profile.Namedatapro))
            {
                MessageBox.Show("Chyba: Pole Namedatapro pre profil " + profile.Namedatapro + " nie je vyplnené");
            }

            if (profile.FontSizeProdatapro < 0 || profile.FontSizeProdatapro > 5)
            {
                MessageBox.Show("Chyba: Pole FontSizeProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.MedzerapredProdatapro < 3 || profile.MedzerapredProdatapro > 6363)
            {
                MessageBox.Show("Chyba: Pole MedzerapredProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.MedzerazaProdatapro < 3 || profile.MedzerazaProdatapro > 6363)
            {
                MessageBox.Show("Chyba: Pole MedzerazaProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.DelenietachaProdatapro < 1 || profile.DelenietachaProdatapro > 63)
            {
                MessageBox.Show("Chyba: Pole DelenietachaProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.PoceopProdatapro < 0 || profile.PoceopProdatapro > 255)
            {
                MessageBox.Show("Chyba: Pole PoceopProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.IntervapopProdatapro < 2 || profile.IntervapopProdatapro > 6363)
            {
                MessageBox.Show("Chyba: Pole IntervapopProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.ExpanseProdatapro < 0 || profile.ExpanseProdatapro > 3)
            {
                MessageBox.Show("Chyba: Pole ExpanseProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.JobdirectionProdatapro < 0 || profile.JobdirectionProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole JobdirectionProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.CharVdirectProdatapro < 0 || profile.CharVdirectProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole CharVdirectProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }
            if (profile.CharHdirectProdatapro < 0 || profile.CharHdirectProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole CharHdirectProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.PrintmodeProdatapro < 0 || profile.PrintmodeProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole PrintmodeProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.SpeedmodeProdatapro < 0 || profile.SpeedmodeProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole SpeedmodeProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.PruductSpeedProdatapro < 2 || profile.PruductSpeedProdatapro > 5745)
            {
                MessageBox.Show("Chyba: Pole PruductSpeedProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

            if (profile.UnitsProdatapro < 0 || profile.UnitsProdatapro > 1)
            {
                MessageBox.Show("Chyba: Pole UnitsProdatapro pre profil " + profile.Namedatapro + " má neplatnú hodnotu");
            }

        }
    }

    public void SaveProfilesdatapro()
    {
        var serializer = new XmlSerializer(typeof(List<Profiledatapro>));
        using (var writer = new StreamWriter(_filePathdatapro))
        {
            serializer.Serialize(writer, _profilesdatapro);
        }


    }

    public List<Profiledatapro> Profilesdatapro
    {
        get { return _profilesdatapro; }
    }

    public void AddProfiledatapro(Profiledatapro profiledatapro)
    {
        _profilesdatapro.Add(profiledatapro);
    }

    public List<Profiledatapro> GetProfilesdatapro()
    {
        return _profilesdatapro;
    }
}

[XmlRoot("Profiledatapro")]
public class Profiledatapro
{
    [XmlElement("Namedatapro")]
    public string Namedatapro { get; set; }
    [XmlElement("FontProIndexdatapro")]
    public int FontSizeProdatapro { get; set; }
    [XmlElement("MedzerapredProdatapro")]
    public int MedzerapredProdatapro { get; set; }
    [XmlElement("MedzerazaProdatapro")]
    public int MedzerazaProdatapro { get; set; }
    [XmlElement("DelenietachaProdatapro")]
    public int DelenietachaProdatapro { get; set; }
    [XmlElement("PoceopProdatapro")]
    public int PoceopProdatapro { get; set; }
    [XmlElement("IntervapopProdatapro")]
    public int IntervapopProdatapro { get; set; }
    [XmlElement("ExpanseProIndexdatapro")]
    public int ExpanseProdatapro { get; set; }
    [XmlElement("JobdirectionProIndexdatapro")]
    public int JobdirectionProdatapro { get; set; }
    [XmlElement("CharVdirectProIndexdatapro")]
    public int CharVdirectProdatapro { get; set; }
    [XmlElement("CharHdirectProIndexdatapro")]
    public int CharHdirectProdatapro { get; set; }
    [XmlElement("PrintmodeProIndexdatapro")]
    public int PrintmodeProdatapro { get; set; }
    [XmlElement("SpeedmodeProIndexdatapro")]
    public int SpeedmodeProdatapro { get; set; }
    [XmlElement("PruductSpeedProdatapro")]
    public int PruductSpeedProdatapro { get; set; }
    [XmlElement("UnitsProIndexdatapro")]
    public int UnitsProdatapro { get; set; }

    public Profiledatapro()
    {
        // Default constructor for serialization purposes
    }

    public Profiledatapro(string namedatapro, int fontSizeProdatapro, int medzerapredProdatapro, int medzerazaProdatapro, int delenietachaProdatapro, int poceopProdatapro, int intervapopProdatapro, int expanseProdatapro, int jobdirectionProdatapro, int charVdirectProdatapro, int charHdirectProdatapro, int printmodeProdatapro, int speedmodeProdatapro, int pruductSpeedProdatapro, int unitsProdatapro)
    {
        Namedatapro = namedatapro;
        FontSizeProdatapro = fontSizeProdatapro;
        MedzerapredProdatapro = medzerapredProdatapro;
        MedzerazaProdatapro = medzerazaProdatapro;
        DelenietachaProdatapro = delenietachaProdatapro;
        PoceopProdatapro = poceopProdatapro;
        IntervapopProdatapro = intervapopProdatapro;
        ExpanseProdatapro = expanseProdatapro;
        JobdirectionProdatapro = jobdirectionProdatapro;
        CharVdirectProdatapro = charVdirectProdatapro;
        CharHdirectProdatapro = charHdirectProdatapro;
        PrintmodeProdatapro = printmodeProdatapro;
        SpeedmodeProdatapro = speedmodeProdatapro;
        PruductSpeedProdatapro = pruductSpeedProdatapro;
        UnitsProdatapro = unitsProdatapro;
    }
}