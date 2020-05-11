# ImageToPDF
�l�X�Ȍ`���̉摜�t�@�C�����APDF (+��)�`���ɕϊ�����B

# �T�v
�ȉ��̌`���̉摜��PDF�`���Ȃǂɕϊ����܂��B

- ���X�^�C���[�W (bmp, png, jpg, gif, tif)
- �x�N�^�C���[�W (wmf, emf, eps)
- �v���[���e�[�V���� (ppt, pptx)

# �����

- Windows 10
- .NET Framework 4.5
- (ppt, pptx��ϊ�����ꍇ) Microsoft Powerpoint 2013 or later
- (wmf, eps, ppt, pptx��eps��pdf�ɕϊ�����ꍇ) [ghostscript] https://www.ghostscript.com/
- (wmf, ppt, pptx��eps��pdf�ɕϊ�����ꍇ) [MetafileToEPSConverter] https://wiki.lyx.org/Windows/MetafileToEPSConverter

# Build from source

1. Clone or download this repository
2. Build the project using VisualStudio 2015 or later.
3. Edit a setting file as described below.
4. Run ImageToPDF.exe in ProjectFolder\bin\Debug\

# Usage

## �R�}���h
```> ImageToPDF.exe [OPTIONS] [FILES|DIRECTORIES]```  

### FILE
�ϊ����̉摜�t�@�C���p�X��0�ȏ�w�肷��B

### DIRECTORIY
�ϊ����̉摜�t�@�C�������݂���f�B���N�g����0�ȏ�w�肷��B  
���̃f�B���N�g�����̂��ׂẲ摜��ϊ�����B

### Option

- ```-w```, ```--allow-overwrite```  
�ϊ���̃t�@�C���Ɠ������O�̃t�@�C�������łɑ��݂���ꍇ�A���̃t�@�C�����㏑������B  
�����w�肵�Ȃ��ꍇ�͏㏑������Ȃ��B

- ```-r```, ```--recursive```  
�ϊ��ΏۂɃf�B���N�g�����w�肵���ꍇ�ɗL���B  
���̃f�B���N�g�����ċA�I�Ɍ������A���݂��邷�ׂẲ摜�t�@�C����ϊ�����B

- ```-t Type```, ```--output-type Type```  
�ϊ���̃t�@�C���`����```Type```�Ɏw�肷��B
    - �����w�肵�Ȃ��ꍇ�A```Type```��pdf�ɂȂ�B
    - �e�ϊ����̃t�@�C���`���ɑ΂��ė��p�\��```Type```�͈ȉ��̕\�̂Ƃ���B
    - �ϊ����s�\�ȑg�ݍ��킹 (bmp��pdf�ɁA�Ȃ�)�͖��������B

<center>

|  �ϊ����̃t�@�C���`��  |  Available ```Type```s  |
| - | - |
|  bmp  |  pdf  |
|  png  |  pdf  |
|  jpg (jpeg)  |  pdf  |
|  gif  |  pdf  |
|  tif (tiff)  |  pdf  |
|  wmf  |  pdf  |
|  emf  |  pdf, eps  |
|  eps  |  pdf  |
|  ppt, pptx  |  pdf, eps, emf  |

</center>

- ```-p start end```, ```--powerpoint--page-range start end```  
Microsoft Powerpoint�̃t�@�C�� (*.ppt, *.pptx)��ϊ�����ꍇ�̂ݗL���B  
�X���C�h��```start```�y�[�W�߂���```end```�y�[�W�߂܂ł̊e�y�[�W���ʂ̉摜�t�@�C���ɕϊ�����B  
    - �ϊ����ʂ̃t�@�C�����́A${�ϊ����̃t�@�C����}${�y�[�W�ԍ�}${�g���q}�ƂȂ�B  
�Ⴆ�΁uImageToPDF.exe -p 5 7 presentation.pptx�v�ƃR�}���h�����s�����ꍇ�A�upresentation5.pdf�v�upresentation6.pdf�v�upresentation7.pdf�v�����ꂼ��o�͂����B
    - �����w�肵�Ȃ��ꍇ�A�X���C�h�̑S�y�[�W���ϊ��ΏۂƂȂ�B

- ```-v```, ```--version```  
�v���O�����̃o�[�W���������o�͂��āA�v���O�������I������B

- ```-L LogLevel```, ```--log-level LogLevel```  
�v���O�������s���ɕ\�����郍�O�̍Œ჌�x����```LogLevel```�Ɏw�肷��B  
���p�\��```LogLevel```��```debug```�A```info```�A```warn```�A```error```�A```none```��5�B  
�����w�肵�Ȃ��ꍇ�A```LogLevel```��```Info```�ƂȂȂ�B  
�����ϊ����ɃG���[�Ȃǂ����������ꍇ�A���̃I�v�V������```Debug```�Ɏw�肵�ďo�͂��m�F����Ɖ����@�������邩������܂���B

## �ݒ�
ImageToPDF.exe�Ɠ��f�B���N�g����settings.txt��ۑ����邱�ƂŁA�ݒ���J�X�^�}�C�Y�ł���B

- EpsConverterPath  
ghostscript�̎��s�t�@�C���p�X���A�uEpsConverterPath=$path�v�̌`���Ŏw�肷��B  
��:  
EpsConverterPath=C:\Program Files\gs\gs9.27\bin\gswin64.exe

- EmfConverterPath  
Metafile to EPS Converter�̎��s�t�@�C���p�X���A�uEmfConverterPath=$path�v�̌`���Ŏw�肷��B  
��:  
EmfConverterPath=C:\Program Files (x86)\Metafile to EPS Converter\metafile2eps.exe

## ���m�̕s�
emf/eps/ppt/pptx��pdf/eps/emf�ɕϊ�����ꍇ�A<u>�ϊ��Ώۂ̃t�@�C���ւ̃t���p�X���󔒂��܂�ł���</u>�Ə������r���Œ�~���Ă��܂��B

# License
MIT

# Author
[Amelia10007] https://github.com/Amelia10007
