ImageToPDF
====

Converts various kinds of image file to PDF.

## Description

ImageToPDF can convert the following kinds of file to PDF:

1. image (*.bmp, *.png, *.jpg, *.jpeg, *.gif, *.wmf)  
Converts the image to 1 page PDF without any margin.

2. Microsoft PowerPoint presentation (*.ppt, *.pptx)  
Converts all objects in each slide to PDFs slide by slide, 
then create PDF with 1 page without any margin.

3. PDF  
Extracts all images in the PDF, then save those as PDFs image by image,
then create PDF with 1 page without any margin.

## Requirement
Windows10  
Microsoft PowerPoint 2013 or later  
.NET Framework 4.5

---

## Usage
In command prompt, type  

"ImageToPDF.exe (filenames | DirectoryNames)"  

* filenames  
ImageToPDF converts these files.
File format is judged by its extension.

* DirectoryNames  
ImageToPDF detects all files in the directories recursively,
then converts those.

ImageToPDF creates PDF in the same directory with the input file.

PDF name is the same with the input file, except its extension.  

If the file with the same name already exists, ImageToPDF creates PDF with an identical name, such as "image(1).pdf".

## Example
In command prompt, if you type

* ImageToPDF.exe image1.jpg  
Converts image1.jpg to image1.pdf

* ImageToPDF.exe image1.jpg image2.png
Converts image1.jpg and image2.png to image1.pdf and image2.pdf, respectively.

* ImageToPDF.exe directory1  
If directory1 has the following structure,  
directory1  
|  
|- image.jpg  
|- presentation.pptx  
|- pdfsource.pdf  
    * converts image.jpg to image.pdf
    * converts all slides in presentation.pptx to presentation.pdf, presentation(1).pdf,...
    * converts all images in pdfsource.pdf to pdfsource(1).pdf, pdfsource(2).pdf,...

---

## Install
1. Clone or download this repository
2. Build the project using VisualStudio 2015 or later.
3. Run ImageToPDF.exe in ProjectFolder\bin\Release\

## Licence
MIT

## Author
[Amelia10007](https://github.com/Amelia10007)