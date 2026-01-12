# C# Audio Player



## Model-View-Controller
![alt text](image.png)

## Depencies
    * FFMpeg
    * Spectre.Console

```sh
dotnet add package Spectre.Console
```
### commands that the wrapper runs
```sh
ffplay -nodisp -autoexit -loglevel quiet "<your_sound_file.wav>"
```