using System.Collections.Generic;

namespace Roulette.Models {
    public class JsonResponse {
        public string Qualifier { get; set; }
        public Dictionary<string, int> Data { get; set; }

    }
}
