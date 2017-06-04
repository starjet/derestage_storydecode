# derestage_storydecode
  
![demo](demo.jpg?raw=true)  
  
Well, at least just the certain day in December part. No traditional Japanese inns where I am.  
  
Anyway, this was started off as a simple tool to rip the text of Starlight Stage commus out of the serialized format that the game uses, mostly utilizing code from the game itself. (Refer to the DerestageClasses namespace.) But since then, more work has been done on it so it could work the other way around as well.  
  
![demo2](demo2.jpg?raw=true) 
  
The program by default will simply rip the text off a serialized binary file and produce a text file with all the deserialized text. However, the following exposed methods in Form1 can do a bit more.  
  
__Form1.decodeRaw(string srcpath):__  
Pass the location of a serialized binary file and this will produce a list of all commands, not just the ones that display story text, in the order that the game will process them.  

__(EXPERIMENTAL) Form1.BuildPlaneFile(string srcpath):__  
This method is made to produce a text file in the "PlaneFile" format that the Parser class from the game can serialize. Pass the location of a serialized binary file and this will produce a text file of the appropriate format.  
  
__Form1.LoadFromPlaneFile(string srcpath):__  
Pass the location of a PlaneFile and this method will serialize it back into binary format.  

Using the latter two methods, entire commus can potentially be translated or even modded in several other ways.  
  
=Original content of this readme=  
Extract story script text from Starlight Stage story data.

Open a single text asset (extract using Unity Studio or DisUnity), or use command line.  
Usage: storydecode.exe path-to-extracted-text-asset
