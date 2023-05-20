using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationAPI
{
    class Response
    {
        public Data data { get; set; }
    }

    class Data
    {
        public Translation[] translations { get; set; }
        public Language[] languages { get; set; }
        public Detection[][] detections { get; set; }
    }

    class Translation
    {
        public string translatedText { get; set; }
        public string detectedSourceLanguage { get; set; }
    }

    class Language
    {
        public string language { get; set; }
    }

    class Detection
    {
        public string language { get; set; }
        public decimal confidence { get; set; }
        public bool isReliable { get; set; }
    }
}
