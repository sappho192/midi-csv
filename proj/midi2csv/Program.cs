using System;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace midi2csv
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("midi2csv can be executed like below:");
                Console.WriteLine("\tdotnet midi2csv.dll C:\\temp\\midifile.midi");
                Console.WriteLine("\tdotnet midi2csv.dll /home/tikim/midifile1.midi /home/tikim/midifile2.midi");
            }
            else
            {
                Console.WriteLine("midi2csv started");
                foreach (var midiLocation in args)
                {
                    Console.WriteLine($"\targ: {midiLocation}");

                    try
                    {
                        var midiFile = MidiFile.Read(midiLocation);
                        var midiNotes = midiFile.GetNotes().ToArray();
                        var csv = new MIDItoCSV();
                        for (int i = 0; i < midiNotes.Length; i++)
                        {
                            var note = midiNotes[i];
                            if (i == 0)
                            {
                                var label = new MidiLabels { 
                                    time = (int)note.Time,
                                    time_diff = 0,
                                    length = (int)note.Length,
                                    note_num = note.NoteNumber,
                                    note_num_diff = 0,
                                    low_octave = note.NoteNumber < 72 ? 1 : 0,
                                    velocity = note.Velocity 
                                };
                                csv.Add(label);
                            }
                            else
                            {
                                var prevNote = midiNotes[i - 1];
                                int timeDiff = (int)(note.Time - prevNote.Time);
								int noteNumDiff = note.NoteNumber - prevNote.NoteNumber;
								var label = new MidiLabels { 
                                    time = (int)note.Time, 
                                    time_diff = timeDiff, 
                                    length = (int)note.Length, 
                                    note_num = note.NoteNumber, 
                                    note_num_diff = noteNumDiff,
                                    low_octave = note.NoteNumber < 72 ? 1 : 0,
                                    velocity = note.Velocity 
                                };
                                csv.Add(label);
                            }
                        }

                        var filename = Path.GetFileNameWithoutExtension(midiLocation);
                        var originalPath = Path.GetDirectoryName(midiLocation);
                        var finalPath = Path.Combine(originalPath, filename);
                        csv.Save(finalPath);
                    }
                    catch (NotEnoughBytesException)
                    {
                        Console.WriteLine("\tmidi2csv failed. Invalid file (not a MIDI file)");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Console.WriteLine("\tmidi2csv failed. Invalid path");
                    }
                }
                Console.WriteLine("midi2csv finished");
            }

            return 0;
        }
    }
}
