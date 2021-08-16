# Traditional Bowling Score System

## Dev environment
.net 5: https://dotnet.microsoft.com/download/dotnet/5.0

Visual Studio 2022 preview: https://visualstudio.microsoft.com/vs/preview/

## Usage
### Request
Endpoint: `POST` `/scores`

Payload
```
{
  "pinsDowned": [int]
}
```

### Response
Body
```
{
  "frameProgressScores": [string],
  "gameCompleted": boolean,
}
```

## Resources
https://www.bowlinggenius.com

https://www.liveabout.com/bowling-scoring-420895

## TODO LIST
[ ] Create classes for frame
