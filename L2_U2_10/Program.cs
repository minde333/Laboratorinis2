using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2_U2_10
{
    class Program
    {
        private const string ChosenManufacturer = "Siemens"; // Pasirenkamas gamintojas su kuriuo skaičiuojami šaldytuvai
        private const string ChosenInstallationType = "Pastatomas"; // Pasirenkamas montavimo tipas pagal kurį daromi skaičiavimai
        private const int CapacityThreshold = 80; // Talpos kiekis pagal kurį reikia palyginti šaldytuvus
        public const int NumberOfBranches = 2; // Parduotuvių skaičius
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Program p = new Program();

            Branch[] branches = new Branch[NumberOfBranches];
            branches[0] = new Branch("Elektroluxas", "Forto g. 5", "862469534");
            branches[1] = new Branch("SAMSUNGAS", "Lekonio g. 64", "861478844");
            
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");

            foreach (string path in filePaths)
            {
                bool rado = p.ReadRefrigeratorData(path, branches);
                if (rado == false)
                    Console.WriteLine("Neatpažintas failo {0} pavadinimas, gatvė ir telefonas", path);
            }

            p.CountSiemens(branches);
            p.PrintRefrigeratorsToConsole(p.SortCheapest(p.TenCheapest(branches)));
            p.ProductsOnlyInOneBranch(branches);
            p.WriteManufacturersToFile(ManufacturerList(branches));
            p.CreateAReportTable(branches, "L1ReportTable.txt");

            Console.ReadKey();
        }
        private bool ReadRefrigeratorData(string file, Branch[] branches)
        {
            string name = null;
            string address = null;
            string phone = null;
            int indexas = 0;
           
            using (StreamReader reader = new StreamReader(@file))
            {
                string line = null;
                line = reader.ReadLine();
                while(line != null && indexas <= NumberOfBranches)
                {
                    if(indexas == 0)
                    {
                        name = line;
                        line = reader.ReadLine();
                        indexas++;
                        continue;
                    }
                    if(indexas == 1)
                    {
                        address = line;
                        line = reader.ReadLine();
                        indexas++;
                        continue;
                    }
                    else
                    if(indexas == 2)
                    {
                        phone = line;
                        indexas++;
                        continue;
                    }
                }
                Branch branch = GetBranchByDifference(branches, name, address, phone);
                if (branch == null)
                {
                    return false;
                }
                while (null != (line = reader.ReadLine()))
                {
                    string[] values = line.Split(',');
                    string Manufacturer = values[0];
                    string Model = values[1];
                    int Capacity = int.Parse(values[2]);
                    string EnergyLabel = values[3];
                    string InstallationType = values[4];
                    string Color = values[5];
                    bool IsThereAFreezer = bool.Parse(values[6]);
                    double Price = double.Parse(values[7]);
                    int Height = int.Parse(values[8]);
                    int Width = int.Parse(values[9]);
                    int Depth = int.Parse(values[10]);

                    Refrigerator refrigerator = new Refrigerator(Manufacturer, Model, Capacity, EnergyLabel, InstallationType,
                                                                 Color, IsThereAFreezer, Price, Height, Width, Depth);

                    branch.Refrigerators.AddRefrigerator(refrigerator);                    
                }
                return true;
            }          
        }

        void CreateAReportTable(Branch[] branches, string file)
        {
            for (int i = 0; i < branches.Count(); i++)
            {
                using (StreamWriter writer = new StreamWriter(@file, true, Encoding.UTF8))
                {
                    writer.WriteLine("Duomenys apie parduotuves ir jų parduodamus šaldytuvus");
                    writer.WriteLine(new string('-', 190));
                    writer.WriteLine("| Parduotuvės pavadinimas: {0, -74} |", branches[i].Name);
                    writer.WriteLine(new String('-', 190));
                    writer.WriteLine("| Parduotuvės adresas: {0, -78} |", branches[i].Address);
                    writer.WriteLine(new String('-', 190));
                    writer.WriteLine("| Parduotuvės telefono numeris: {0, -69} |", branches[i].Phone);
                    writer.WriteLine(
                        "| {0, -15} | {1, -15} | {2, -11} | {3, -10} | {4, -6} | {5, -10} | {6, -13} | {7, -15} | {8, -16} | {9, -17}" +
                        "| {10, -16} |", "Gamintojas", "Modelis", "Talpa", "Energijos klasė", "Montavimo tipas", "Spalva", "Šaldiklis",
                        "Kaina", "Aukštis", "Plotis", "Gylis");
                    writer.WriteLine(new string('-', 190));

                    for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                    {
                        writer.WriteLine("| {0, -15} | {1, -15} | {2, 11} | {3, -15} | {4, -15} | {5, -10} | {6, -13} | {7, 15}" +
                            " | {8, 16} | {9, 17} | {10, 16} |", branches[i].Refrigerators.GetRefrigerator(j).Manufacturer,
                            branches[i].Refrigerators.GetRefrigerator(j).Model, branches[i].Refrigerators.GetRefrigerator(j).Capacity,
                            branches[i].Refrigerators.GetRefrigerator(j).EnergyLabel, branches[i].Refrigerators.GetRefrigerator(j).InstallationType,
                            branches[i].Refrigerators.GetRefrigerator(j).Color, branches[i].Refrigerators.GetRefrigerator(j).IsThereAFreezer,
                            branches[i].Refrigerators.GetRefrigerator(j).Price, branches[i].Refrigerators.GetRefrigerator(j).Height,
                            branches[i].Refrigerators.GetRefrigerator(j).Width, branches[i].Refrigerators.GetRefrigerator(j).Depth);
                        writer.WriteLine(new string('-', 190));
                    }
                }
            }
        }

            private Branch GetBranchByDifference(Branch[] branches, string name, string address, string phone)
        {
            for (int i = 0; i < NumberOfBranches; i++)
            {
                if (branches[i].Name == name && branches[i].Address == address && branches[i].Phone == phone)
                {
                    return branches[i];
                }
            }
            return null;
        }
        void PrintRefrigeratorsToConsole(RefrigeratorContainer refrigerators)
        {
            Console.WriteLine("10 pigiausių " + "'" + ChosenInstallationType + "'" + " montavimo tipų šaldytuvų, kurių talpa " +
                              CapacityThreshold + " litrų ar didesnė, sąrašas:\n");

            for (int i = 0; i < refrigerators.Count; i++)
            {
                Console.WriteLine("| Nr. {0, 2} | {1}", (i + 1), refrigerators.GetRefrigerator(i).ToString());
            }
        }
        public void CountSiemens(Branch[] branches)
        {
            List<string> uniqueModels1 = new List<string>();//pirmo brancho
            List<string> uniqueModels2 = new List<string>();//antro brancho

            for (int i = 0; i < branches.Count(); i++)
            {
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {
                    if (i == 0)
                    {
                        if (branches[i].Refrigerators.GetRefrigerator(j).Manufacturer == ChosenManufacturer &&
                            !uniqueModels1.Contains(branches[i].Refrigerators.GetRefrigerator(j).Model))
                        {
                            uniqueModels1.Add(branches[i].Refrigerators.GetRefrigerator(j).Model);
                        }
                    }
                    else if (branches[i].Refrigerators.GetRefrigerator(j).Manufacturer == ChosenManufacturer &&
                             !uniqueModels2.Contains(branches[i].Refrigerators.GetRefrigerator(j).Model))
                         {
                            uniqueModels2.Add(branches[i].Refrigerators.GetRefrigerator(j).Model);
                         }
                }
            }
            PrintModels(uniqueModels1,uniqueModels2);
        }

        public void PrintModels(List<string> uniqueModels1, List<string> uniqueModels2)
        {
            Console.WriteLine("Skirtingų " + ChosenManufacturer + " šaldytuvų modelių kiekviena parduotuvė siūlo:\n ");
            Console.WriteLine("Elektroluxas: " + uniqueModels1.Count());
            Console.WriteLine("SAMSUNGAS: " + uniqueModels2.Count());
            Console.WriteLine();
        }

        public Branch TenCheapest(Branch[] branches)
        {
            bool dontAdd = false;
            int firstAdd = 0;
            Branch cheapest = new Branch(null,null,null);
            
            for (int i = 0; i < branches.Count(); i++)
            {
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {                 
                    if(branches[i].Refrigerators.GetRefrigerator(j).Capacity >= CapacityThreshold &&
                       branches[i].Refrigerators.GetRefrigerator(j).InstallationType == ChosenInstallationType)
                    {
                        dontAdd = false;
                        if(firstAdd == 0)
                        {
                            cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            firstAdd++;
                            continue;
                        }
                        if (i == 1)
                        {
                            for (int k = 0; k < cheapest.Refrigerators.Count; k++)
                            {
                                if (cheapest.Refrigerators.GetRefrigerator(k).Equals(branches[i].Refrigerators.GetRefrigerator(j)) &&
                                    cheapest.Refrigerators.GetRefrigerator(k).Equals(branches[i].Refrigerators.GetRefrigerator(j)))
                                {
                                    dontAdd = true;
                                    break;
                                }
                            }
                            if(dontAdd == false)
                            {
                                cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                            }                          
                        }
                        else
                            cheapest.Refrigerators.AddRefrigerator(branches[i].Refrigerators.GetRefrigerator(j));
                        dontAdd = false;
                    }
                }
            }
            return cheapest;
        }
        public RefrigeratorContainer SortCheapest(Branch cheapest)
        {
            Refrigerator temp = new Refrigerator();
            for (int i = 0; i < cheapest.Refrigerators.Count; i++)
            {
                for (int j = 0; j < cheapest.Refrigerators.Count; j++)
                {
                    if (cheapest.Refrigerators.GetRefrigerator(i) <= cheapest.Refrigerators.GetRefrigerator(j))
                    {
                        temp = cheapest.Refrigerators.GetRefrigerator(i);
                        cheapest.Refrigerators.AddRefrigerator(cheapest.Refrigerators.GetRefrigerator(j), i);
                        cheapest.Refrigerators.AddRefrigerator(temp, j);
                    }
                }
            }
            return cheapest.Refrigerators;
        }
        public void ProductsOnlyInOneBranch(Branch[] branches)
        {
            bool dontProcces = false;
            List<string> manufacturersElektro = new List<string>();
            manufacturersElektro.Add("Elektroluxas");
            List<string> manufacturersSams = new List<string>();
            manufacturersSams.Add("SAMSUNGAS");

            for (int i = 0; i < branches.Count(); i++)
            {
                if(i == 0)
                {
                    for (int j = 0; j < branches[0].Refrigerators.Count; j++)
                    {
                        for (int k = 0; k < branches[1].Refrigerators.Count; k++)
                        {
                            if (branches[0].Refrigerators.GetRefrigerator(j).Equals(branches[1].Refrigerators.GetRefrigerator(k)))
                            {
                                dontProcces = true;
                                break;
                            }
                        }
                        if (!manufacturersElektro.Contains(branches[0].Refrigerators.GetRefrigerator(j).Manufacturer) && dontProcces == false)
                        {
                            manufacturersElektro.Add(branches[0].Refrigerators.GetRefrigerator(j).Manufacturer);
                        }
                    }
                }
                else
                        for (int j = 0; j < branches[1].Refrigerators.Count; j++)
                        {
                        dontProcces = false;
                            for (int k = 0; k < branches[0].Refrigerators.Count; k++)
                            {
                                if (branches[1].Refrigerators.GetRefrigerator(j).Equals(branches[0].Refrigerators.GetRefrigerator(k)))
                                {
                                dontProcces = true;
                                break;
                                }
                            }
                            if(!manufacturersSams.Contains(branches[1].Refrigerators.GetRefrigerator(j).Manufacturer) && dontProcces == false)
                            {
                               manufacturersSams.Add(branches[1].Refrigerators.GetRefrigerator(j).Manufacturer);
                            }                            
                        }                    
            }
            PrintUnique(manufacturersElektro, manufacturersSams);     
        }
        public void PrintUnique(List<string> manufacturersElektro, List<string> manufacturersSams)
        {
            using (StreamWriter writer = new StreamWriter(@"TikTen.csv", false, Encoding.UTF8))
            {
                if (manufacturersElektro.Count > 1)
                {
                writer.WriteLine(manufacturersElektro.First() + "\n");
                    for (int i = 1; i < manufacturersElektro.Count; i++)
                    {
                        writer.WriteLine(manufacturersElektro[i]);
                    }
                }
                if (manufacturersSams.Count > 1)
                {
                    writer.WriteLine(manufacturersSams.First() + "\n");
                    for (int i = 1; i < manufacturersSams.Count; i++)
                    {
                        writer.WriteLine(manufacturersSams[i]);
                    }
                }
            }
        }
        public static List<string> ManufacturerList(Branch[] branches)
        {
            List<string> manufacturers = new List<string>();

            for (int i = 0; i < branches.Count(); i++)
            {
                for (int j = 0; j < branches[i].Refrigerators.Count; j++)
                {
                    if(!manufacturers.Contains(branches[i].Refrigerators.GetRefrigerator(j).Manufacturer))
                    {
                        manufacturers.Add(branches[i].Refrigerators.GetRefrigerator(j).Manufacturer);
                    }
                }
            }
            return manufacturers;
        }  
        public void WriteManufacturersToFile(List<string> manufacturers)
        {
            using (StreamWriter writer = new StreamWriter(@"Gamintojai.csv", false, Encoding.UTF8))
            {
                for (int i = 0; i < manufacturers.Count; i++)
                {
                    writer.WriteLine(manufacturers[i]);
                }
            }
        }
    }
}
