# midi-csv
Utility program which converts data between MIDI and CSV files.

## Requirements
- For development: .NET SDK 6.0
- For end-user: .NET 6.0 Runtime
Download link: https://dotnet.microsoft.com/ko-kr/download/dotnet/6.0

## midi2csv
Reads MIDI file and saves following data into CSV file:
- Timestamp (time)
- Timestamp difference with previous note (time_diff)
- Note Number (note_num)
- Length (length)
- Velocity (velocity)

Note that Length is calculated value by subtracting NoteOff and NoteOn time.
Every data type is integer, but range is different:
- time, time_diff, length: Maybe int32?
- note_num, velocity: [0,127]

Example of csv text file:
```
time,time_diff,note_num,length,velocity
240,0,65,1920,36
240,0,68,1920,43
```

## csv2midi
Reads CSV file and generates MIDI file.
CSV file must contain following column data:
- Timestamp (time)
- Timestamp difference with previous note (time_diff)
- Note Number (note_num)
- Length (length)
- Velocity (velocity)

Of course there are many other types of data you can retrieve from MIDI, but since this program is made for my another personal project, only mentioned data are manipulated.  
But you can always modify the source code to make your own program from this.
