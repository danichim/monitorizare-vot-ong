﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;
using ExcelDataReader.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MonitorizareVot.Domain.Ong.Models;
using MonitorizareVot.Domain.Ong.ValueObjects;


namespace MonitorizareVot.Domain.Ong
{
    public static class USROngContextExtensions
    {

      
        public static void EnsureUSRSeedData(this OngContext context)
        {

            var intrebariExcel = ExcelParser.GetIntrebariObservatorFromFile();
            List<Optiune> optiuni = GetOptiuni(intrebariExcel);
            List<Sectiune> sectiuni = GetSectiuni(intrebariExcel);
            List<Intrebare> intrebari = GetIntrebariObservator(intrebariExcel, optiuni, sectiuni);

            if (!context.AllMigrationsApplied())
                return;

            using (var tran = context.Database.BeginTransaction())
            {
                context.DataCleanUp();
                context.SeedVersions();
                context.SeedJudete();
                context.SeedSectiune(sectiuni);
                context.SeedOptiuni(optiuni);
                context.SeedIntrebariObservator(intrebari);


                tran.Commit();
            }
        }


        private static void SeedIntrebariObservator(this OngContext context, List<Intrebare> intrebari)
        {

            if (context.Intrebare.Any())
                return;
            
            context.Intrebare.AddRange(intrebari);
            context.SaveChanges();
        }



        private static void SeedJudete(this OngContext context)
        {
            if (context.Judet.Any())
                return;

            context.Judet.AddRange(
                new Judet { IdJudet = 1, CodJudet = "AB", Nume = "ALBA" },
                new Judet { IdJudet = 2, CodJudet = "AR", Nume = "ARAD" },
                new Judet { IdJudet = 3, CodJudet = "AG", Nume = "ARGES" },
                new Judet { IdJudet = 4, CodJudet = "BC", Nume = "BACAU" },
                new Judet { IdJudet = 5, CodJudet = "BH", Nume = "BIHOR" },
                new Judet { IdJudet = 6, CodJudet = "BN", Nume = "BISTRITA-NASAUD" },
                new Judet { IdJudet = 7, CodJudet = "BT", Nume = "BOTOSANI" },
                new Judet { IdJudet = 8, CodJudet = "BV", Nume = "BRASOV" },
                new Judet { IdJudet = 9, CodJudet = "BR", Nume = "BRAILA" },
                new Judet { IdJudet = 10, CodJudet = "BZ", Nume = "BUZAU" },
                new Judet { IdJudet = 11, CodJudet = "CS", Nume = "CARAS-SEVERIN" },
                new Judet { IdJudet = 12, CodJudet = "CL", Nume = "CALARASI" },
                new Judet { IdJudet = 13, CodJudet = "CJ", Nume = "CLUJ" },
                new Judet { IdJudet = 14, CodJudet = "CT", Nume = "CONSTANTA" },
                new Judet { IdJudet = 15, CodJudet = "CV", Nume = "COVASNA" },
                new Judet { IdJudet = 16, CodJudet = "DB", Nume = "DÂMBOVITA" },
                new Judet { IdJudet = 17, CodJudet = "DJ", Nume = "DOLJ" },
                new Judet { IdJudet = 18, CodJudet = "GL", Nume = "GALATI" },
                new Judet { IdJudet = 19, CodJudet = "GR", Nume = "GIURGIU" },
                new Judet { IdJudet = 20, CodJudet = "GJ", Nume = "GORJ" },
                new Judet { IdJudet = 21, CodJudet = "HR", Nume = "HARGHITA" },
                new Judet { IdJudet = 22, CodJudet = "HD", Nume = "HUNEDOARA" },
                new Judet { IdJudet = 23, CodJudet = "IL", Nume = "IALOMITA" },
                new Judet { IdJudet = 24, CodJudet = "IS", Nume = "IASI" },
                new Judet { IdJudet = 25, CodJudet = "IF", Nume = "ILFOV" },
                new Judet { IdJudet = 26, CodJudet = "MM", Nume = "MARAMURES" },
                new Judet { IdJudet = 27, CodJudet = "MH", Nume = "MEHEDINTI" },
                new Judet { IdJudet = 28, CodJudet = "B", Nume = "MUNICIPIUL BUCURESTI" },
                new Judet { IdJudet = 29, CodJudet = "MS", Nume = "MURES" },
                new Judet { IdJudet = 30, CodJudet = "NT", Nume = "NEAMT" },
                new Judet { IdJudet = 31, CodJudet = "OT", Nume = "OLT" },
                new Judet { IdJudet = 32, CodJudet = "PH", Nume = "PRAHOVA" },
                new Judet { IdJudet = 33, CodJudet = "SM", Nume = "SATU MARE" },
                new Judet { IdJudet = 34, CodJudet = "SJ", Nume = "SALAJ" },
                new Judet { IdJudet = 35, CodJudet = "SB", Nume = "SIBIU" },
                new Judet { IdJudet = 36, CodJudet = "SV", Nume = "SUCEAVA" },
                new Judet { IdJudet = 37, CodJudet = "TR", Nume = "TELEORMAN" },
                new Judet { IdJudet = 38, CodJudet = "TM", Nume = "TIMIS" },
                new Judet { IdJudet = 39, CodJudet = "TL", Nume = "TULCEA" },
                new Judet { IdJudet = 40, CodJudet = "VS", Nume = "VASLUI" },
                new Judet { IdJudet = 41, CodJudet = "VL", Nume = "VÂLCEA" },
                new Judet { IdJudet = 42, CodJudet = "VN", Nume = "VRANCEA" }
                );
        }

        private static void DataCleanUp(this OngContext context)
        {
            context.Database.ExecuteSqlCommand("truncate table Nota");
            context.Database.ExecuteSqlCommand("truncate table NotaLipsa");
            context.Database.ExecuteSqlCommand("truncate table Raspuns");
            context.Database.ExecuteSqlCommand("delete from RaspunsDisponibil");
            context.Database.ExecuteSqlCommand("delete from Intrebare");
            context.Database.ExecuteSqlCommand("delete from Sectiune");
            context.Database.ExecuteSqlCommand("delete from VersiuneFormular");
            context.Database.ExecuteSqlCommand("delete from Optiune");
        }

        private static void SeedOptiuni(this OngContext context, List<Optiune> optiuni)
        {
            if (context.Optiune.Any())
                return;
            context.Optiune.AddRange(
                optiuni
            );

            context.SaveChanges();
        }

        private static void SeedSectiune(this OngContext context, List<Sectiune> sectiuni)
        {
            if (context.Sectiune.Any())
                return;

            context.Sectiune.AddRange(
               sectiuni
                );

            context.SaveChanges();
        }

       

            private static void SeedVersions(this OngContext context)
        {
            if (context.VersiuneFormular.Any())
                return;

            context.VersiuneFormular.AddRange(
                 new VersiuneFormular { CodFormular = "A", VersiuneaCurenta = 2 },
                 new VersiuneFormular { CodFormular = "B", VersiuneaCurenta = 2 },
                 new VersiuneFormular { CodFormular = "C", VersiuneaCurenta = 2 }
             );

            context.SaveChanges();
        }

     

        private static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }


        //Mappers 

        private static List<Optiune> GetOptiuni(List<IntrebareExcel> intrebariExcel)
        {
           
           
            var optiuni = intrebariExcel.SelectMany(x => x.Optiuni)
                .GroupBy(p => new { p.Text, p.SeIntroduceText })
                .Select(g => g.First())
                .Select((optiuneExcel, index) => 
                new Optiune { IdOptiune = index + 1, TextOptiune = optiuneExcel.Text, SeIntroduceText = optiuneExcel.SeIntroduceText }
                ).ToList();
          
            return optiuni;
        }

        private static List<Sectiune> GetSectiuni(List<IntrebareExcel> intrebariExcel)
        {


            var sectiuni = intrebariExcel.Select(x => x.IdSectiune).Distinct()
                .Select((text, index) =>
                new Sectiune { IdSectiune = index + 1, CodSectiune = text.Split('-')[0], Descriere = text.Split('-')[1] }
                )
                .ToList();

            return sectiuni;
        }

        private static List<Intrebare> GetIntrebariObservator(List<IntrebareExcel> intrebariExcel, List<Optiune> optiuni, List<Sectiune> sectiuni)
        {
            var intrebari = intrebariExcel.Select((x, index) => new Intrebare
            {
                IdIntrebare = index + 1,
                CodIntrebare = x.CodFormular,
                CodFormular = x.NewCodFormular,
                IdSectiune = GetIdSectiune(x.IdSectiune, sectiuni),
                IdTipIntrebare = GetTipIntrebare(x.IdTipIntrebare),
                TextIntrebare = x.CodIntrebare + " " + x.TextIntrebare,
                RaspunsDisponibil = x.Optiuni.Select((y, indexOptiune) => new RaspunsDisponibil
                {
                    IdRaspunsDisponibil = index * 1000 + indexOptiune,
                    IdOptiune = GetIdOptiune(y, optiuni),
                    RaspunsCuFlag = y.HasFlag
                }).ToList()
            }).ToList();
            return intrebari;
        }


        //HELPERS

        private static int GetTipIntrebare(string cod)
        {
            Dictionary<string, int> hash = new Dictionary<string, int>() {
                { "single choice",  TipIntrebareEnum.OSinguraOptiune},
                { "single choice cu text",  TipIntrebareEnum.OSinguraOptiuneCuText},
                { "multiple choice",  TipIntrebareEnum.OptiuniMultiple},
                { "multiple choice cu text",  TipIntrebareEnum.OptiuniMultipleCuText}
            };

            return hash[cod];
        }


        private static int GetIdOptiune(OptiuneExcel optiuneExcel, List<Optiune> optiuni)
        {
            return optiuni.SingleOrDefault(x => x.TextOptiune == optiuneExcel.Text && x.SeIntroduceText == optiuneExcel.SeIntroduceText).IdOptiune;
        }

        private static int GetIdSectiune(String sectiuneExcel, List<Sectiune> sectiuni)
        {
            return sectiuni.SingleOrDefault(x => x.CodSectiune == sectiuneExcel.Split('-')[0]).IdSectiune;
        }


    }
}
