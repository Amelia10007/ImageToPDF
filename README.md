ImageToPDF
====

Converts various kinds of image file to PDF.

## Description

ImageToPDF can convert the following kinds of file to PDF:

1. raster image  
bmp, png, jpg (jpeg), gif and tif (tiff) are available.

2. vector image  
wmf, emf and eps are available.

3. Microsoft PowerPoint presentation (*.ppt, *.pptx)  
Converts all objects in each slide to PDF slide by slide without margin.

4. PDF  
Extracts all images in the PDF, then save those image by image.

---

## Requirement
Windows10  
Microsoft PowerPoint 2013 or later  
.NET Framework 4.5  
[ghostscript] https://www.ghostscript.com/  
[MetafileToEPSConverter] https://wiki.lyx.org/Windows/MetafileToEPSConverter

---

## Install
1. Clone or download this repository
2. Build the project using VisualStudio 2015 or later.
3. Edit a setting file as described below.
4. Run ImageToPDF.exe in ProjectFolder\bin\Release\

## Setting
Before you run ImageToPDF, you have to prepare a setting file "settings.txt" at the same directory with the executable.  

"settings.txt" has to contain the locations of ghostscript and MetafileToEPSConverter. Its format will be like this:  

EpsConverterPath = C:\Program Files\ghostscript\executable.exe  
EmfConverterPath = C:\Program Files\emf2eps.exe

---

## Usage
In command prompt, type  

"ImageToPDF.exe (filenames | DirectoryNames | options)"  

* filenames  
ImageToPDF converts these files.
File format is judged by its extension.

* DirectoryNames  
ImageToPDF detects all files in the directories and converts those.

* options  
    * DirectoryName -r  
    ImageToPDF detects all files in the directory recursively.
    * PowerpointFilename page  
    ImageToPDF converts the page-th slide in the powerpoint (page must be an integer).
    * PowerpointFilename start end  
    ImageToPDF converts the slides from start-th to end-th (these options must be integers).

ImageToPDF creates PDF in the same directory with the input file.

Generally PDF name is the same with the source file except its extension.  

If the file with the same name already exists, it will be overwritten.

## Example
In command prompt, if you type

* ImageToPDF.exe image1.jpg image2.png
Converts image1.jpg and image2.png to image1.pdf and image2.pdf, respectively.

* ImageToPDF.exe presentation.pptx 5  
Converts 5th slide in presentation.pptx.

* ImageToPDF.exe presentation.pptx 3 10  
Converts from 3rd to 10th slides in presentation.pptx. 8 (= 10 - 3 +1) PDF files will be generated.

* ImageToPDF.exe directory  
If the directory has the following structure,  
directory  
|  
|- image.jpg  
|- presentation.pptx  
    * converts image.jpg to image.pdf
    * converts all slides in presentation.pptx to presentation1.pdf, presentation2.pdf,...

* ImageToPDF.exe parent -r  
If the directory has the following structure,  
parent  
|  
|- child1  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- image1.jpg  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- presentation1.pptx  
|- child2  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- image2.png  
|- image.gif

    ImageToPDF converts child1/image1.jpg, child1/presentation1.pptx, child2/image.png and image.gif

---


## Licence
MIT

## Author
[Amelia10007](https://github.com/Amelia10007)