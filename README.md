# Traditional Bowling Score System

## Dev environment
.net 6 preview-7: https://dotnet.microsoft.com/download/dotnet/6.0

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
