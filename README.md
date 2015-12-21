![Picture](res/logo.png?raw=true)  
Simple YouTube music manager

**Welcome to the SYMM Base repo!**  
You're here because Google brought you here, or you where just lurking around on Github, weren't you? Gotcha. Okay so let me tell what SYMM is.  
SYMM is a project which is split in two parts: The frontends and a single backend.  
This readme is about the _backend_. _(checkout the frontends under /src for more info about them)_  
Let's say you would like to develope an application, which is able to 
- stream the audio
- extract the audio
- download the video

_of_  
- a single video (If you're lame enough)
- a playlist (You're getting cooler)
- a whole channel (Now we're talking.) 
  
from YouTube.  
With this libary you can, pretty easiliy. With the help of this libary it's just a matter of setting up a settings object, which stores all info about a download, as an example the URL of the video and some settings about quality and output format. After that you call _PrepareDownloadL()_ passing the settings to fetch general data about the video, register some events so you can keep up with the current download status and _Execute()_ to start downloading your video(s). If you wisch the video(s) gets automaticly converted to audio afterwards with the power of _ffmpeg_.  
**Done.**  
  
When your application uses this backend, than you did nothing else than developing a new _Frontend_ for SYMM. Frontends are the Interface's that trigger the actual work in the backend. Just like you know it from web applications.  
  
## Contributing
### Contributing general
Fork this repo, do your work and open a pull request. Please only do not commit changes that mean something, e.g. don't commit a solution file because VisualStudio decided to change a version number. Pull request which contain such changes wont be accepted.  
