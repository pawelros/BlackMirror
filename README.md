# BlackMirror

Web Application that synchronizes your Git and SVN repositories in realtime.
This application saves your time by allowing you to add new mirrors directly from the Web GUI, track the progress and many more.

This repository consist of:
- REST WebAPI written in C#
- BlackMirror Worker (agent that is supposed to be running on a server and do dirty work of synchronizing your repositories)

Requirements:
- Running MongoDB instance
- [black-mirror-ui](https://github.com/Rosiv/black-mirror-ui "black-mirror-ui")

More detailed documentation TBD. The solution does not build due to missing dependencies. I am working on a fix.