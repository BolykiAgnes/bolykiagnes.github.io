using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.ViewModels
{
    public class PlateRecognizerResponse
    {
        public List<Result> Results { get; set; }
        public string Filename { get; set; }
        public DateTime Timestamp { get; set; }

    }

    public class Region
    {
        public string Code { get; set; }
    }

    public class Vehicle
    {
        public string Type { get; set; }
    }

    public class Candidate
    {
        public double Score { get; set; }
        public string Plate { get; set; }
    }
    public class Result
    {
        public string Plate { get; set; }
        public Region Region { get; set; }
        public Vehicle Vehicle { get; set; }
        public List<Candidate> Candidates { get; set; }
    }
}
