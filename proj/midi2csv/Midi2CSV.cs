using System.Collections.Generic;
using System.IO;
using System.Text;

namespace midi2csv
{
    public class MIDItoCSV
    {
        public readonly string fields;
        private List<MidiLabels> noteInfos;

        public MIDItoCSV()
        {
            fields = $"{nameof(MidiLabels.time)},{nameof(MidiLabels.time_diff)},{nameof(MidiLabels.note_num)},{nameof(MidiLabels.note_num_diff)},{nameof(MidiLabels.low_octave)},{nameof(MidiLabels.length)},{nameof(MidiLabels.velocity)}\n";
            noteInfos = new List<MidiLabels>();
        }

        public void Add(MidiLabels result)
        {
            noteInfos.Add(result);
        }

        public bool Save(string filename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(fields);
            foreach (var item in noteInfos)
            {
				sb.Append($"{item.time},{item.time_diff},{item.note_num},{item.note_num_diff},{item.low_octave},{item.length},{item.velocity}\n");
			}

            File.WriteAllText($"{filename}.csv", sb.ToString());

            return true;
        }
    }
}
