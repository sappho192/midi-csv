using CsvHelper;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace csv2midi
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("csv2midi can be executed like below:");
                Console.WriteLine("  dotnet midi2csv.dll C:\\temp\\midifile_predicted.csv C:\\temp\\midifile.mid");
                Console.WriteLine("  dotnet midi2csv.dll /home/tikim/midifile_predicted.csv /home/tikim/midifile.mid");
                Console.WriteLine("Result file will be saved like 'midifile_predicted.mid'");
            }
            else
            {
                Console.WriteLine("csv2midi started");
                var csvLocation = args[0];
                var midiLocation = args[1];

                using TextReader reader = File.OpenText(csvLocation);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                List<int> velocities = csv.GetRecords<VelocityLabel>().Select(v => v.velocity).ToList();

                try
                {
                    var midiFile = MidiFile.Read(midiLocation);

                    //using (NotesManager notesManager = midiFile.GetTrackChunks().First().ManageNotes())
                    /* In MAESTRO midi dataset, there were 2 note(?) events.
                    * The size of event is very small for one and very large for another.
                    * Since the larger one is the very note event that I want to manipulate,
                    * Following Aggregate() Linq code is used to choose the larger one simply.
                    */
                    using (NotesManager notesManager = midiFile.GetTrackChunks().Aggregate((c1, c2) => c1.Events.Count > c2.Events.Count ? c1 : c2).ManageNotes())
                    {
                        int i = 0;
                        foreach (var note in notesManager.Notes)
                        {
                            note.Velocity = (SevenBitNumber)velocities[i];
                            i++;
                        }
                    }

                    var filename = Path.GetFileNameWithoutExtension(midiLocation);
                    var extension = Path.GetExtension(midiLocation);
                    var originalPath = Path.GetDirectoryName(midiLocation);
                    var finalPath = Path.Combine(originalPath, $"{filename}_predicted{extension}");
                    midiFile.Write(finalPath, overwriteFile: true);
                }
                catch (NotEnoughBytesException)
                {
                    Console.WriteLine("\tcsv2midi failed. Invalid file (not a MIDI file)");
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("\tcsv2midi failed. Invalid path");
                }

                Console.WriteLine("csv2midi finished");
            }

            return 0;
        }
    }
}
