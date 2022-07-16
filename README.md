
# AnimOfDots (Loading Indicator)
[.NET WinForms] Animation of loading with dots for C# and VB.NET
#
Preview:

<img src="https://raw.githubusercontent.com/mt-alts/AnimOfDots/main/preview.gif"/>

<table>
  <tr>
    <td>Pulse</td>
    <td>Circular</td>
    <td>DotScaling</td>
  </tr>
  <tr>
    <td>DotGridFlashing</td>
    <td>MultiplePulse</td>
    <td>DoubleDotSpin</td>
  </tr>
    <tr>
    <td>DotTyping</td>
    <td>Overlay</td>
    <td>DotFlashing</td>
  </tr>
</table>

#

## Properties and Methods
### Common property and methods
| Property or Method | Type  | Description |
| :-------- | :------- | :------------------------- |
| AnimationSpeed | Property | Changes animation speed |
| BackColor | Property | Changes the background color of the indicator |
| Start() | Method | Start animating |
| Stop() | Method | Stop animating |
#
#### Circular, DotGridFlashing, DotTyping, DoubleDotSpin, Pulse, MultiplePulse
| Property or Method | Type | Description  |
| :-------- | :------- | :------------------------- |
| ForeColor | Property | Changes the color of the indicator |
#
#### Overlay, ColorfulCircular, DotScaling
| Property or Method | Type | Description   |
| :-------- | :------- | :------------------------- |
| Colors | Property | Changes the color of the indicator <br /> according to the color array |
#
#### DotGridFlashing
| Property or Method | Type | Description |
| :-------- | :------- | :------------------------- |
| ColorAlpha | Property | Changes the transparency level of <br /> half of the indicator elements |
#
#### DotFlashing, DoubleDotSpin
| Property or Method | Type | Description |
| :-------- | :------- | :------------------------- |
| PrimaryColor | Property | Changes the primary color         |
| SecondaryColor | Property | Changes the secondary color     |
#
 ℹ️ Some controls must have the same aspect ratio to work properly.</br>
⚠️ Performance loss may occur if the animation speed is set too high.
