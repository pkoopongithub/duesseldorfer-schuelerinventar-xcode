using System.Text.Json.Serialization;

namespace DüsseldorferSchülerinventar.Models
{
    public class Profile
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonIgnore]
        public int[] Answers { get; set; } = new int[36];

        [JsonIgnore]
        public int[] CompetenceScores { get; private set; } = new int[6];

        public void CalculateScores(double[][] normTable)
        {
            // Arbeitsverhalten (Items 1-10)
            CompetenceScores[0] = CalculateCompetenceScore(new[] {0,1,2,3,4,5,6,7,8,9}, normTable[0]);
            
            // Lernverhalten (Items 11-20)
            CompetenceScores[1] = CalculateCompetenceScore(new[] {10,11,12,13,14,15,16,17,18,19}, normTable[1]);
            
            // Sozialverhalten (Items 21-28 + 8,9)
            CompetenceScores[2] = CalculateCompetenceScore(new[] {20,21,22,23,24,25,26,27,8,9}, normTable[2]);
            
            // Fachkompetenz (Items 29-36)
            CompetenceScores[3] = CalculateCompetenceScore(new[] {28,29,30,31,32,33,34,35}, normTable[3]);
            
            // Personale Kompetenz
            CompetenceScores[4] = CalculateCompetenceScore(new[] {0,1,5,6,7,8,9,10,11,13,14}, normTable[4]);
            
            // Methodenkompetenz
            CompetenceScores[5] = CalculateCompetenceScore(new[] {2,3,4,8,9,10,16,17}, normTable[5]);
        }

        private int CalculateCompetenceScore(int[] itemIndices, double[] normValues)
        {
            int sum = itemIndices.Sum(i => Answers[i]);
            
            for (int i = 0; i < normValues.Length; i++)
            {
                if (sum < normValues[i])
                    return i + 1; // 1-5 Skala
            }
            return normValues.Length;
        }
    }
}