namespace playground
{
    class Tournament
    {
        public string Name { get; set; }
        public List<string> Participants { get; set; }

        public Tournament(string name)
        {
            Name = name;
            Participants = new List<string>();
        }

        public void AddParticipant(string name)
        {
            Participants.Add(name);
        }

        public void StartTournament()
        {
            Console.WriteLine($"The {Name} tournament is starting!");

            for (int i = 0; i < Participants.Count; i += 2)
            {
                Console.WriteLine($"Match {i / 2 + 1}: {Participants[i]} vs. {Participants[i + 1]}");
            }
        }

        public static void Run()
        {
            Tournament tournament = new Tournament("Karate Championship");
            tournament.AddParticipant("John Smith");
            tournament.AddParticipant("Jane Doe");
            tournament.AddParticipant("Bob Johnson");
            tournament.AddParticipant("Sue Williams");
            tournament.AddParticipant("Samuel Thompson");
            tournament.AddParticipant("Jessica Brown");

            tournament.StartTournament();
            Console.ReadLine();
        }
    }
}
