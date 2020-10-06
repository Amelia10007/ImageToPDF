# ImageToPDF
様々な形式の画像ファイルを、PDF (+α)形式に変換する。

# 概要
以下の形式の画像をPDF形式などに変換します。

- ラスタイメージ (bmp, png, jpg, gif, tif)
- ベクタイメージ (wmf, emf, eps)
- プレゼンテーション (ppt, pptx)

# 動作環境

- Windows 10
- .NET Framework 4.5
- (ppt, pptxを変換する場合) Microsoft Powerpoint 2013 or later
- (wmf, eps, ppt, pptxをepsやpdfに変換する場合) [ghostscript] https://www.ghostscript.com/
- (wmf, ppt, pptxをepsやpdfに変換する場合) [MetafileToEPSConverter] https://wiki.lyx.org/Windows/MetafileToEPSConverter

# ビルド

1. このリポジトリをクローン
1. .slnをVisual Studio 2015以上で開く
1. NuGetパッケージマネージャーで「iTextSharp」をソリューションに追加
1. ビルド
1. 設定ファイルを以下の説明の通りに編集する
1. このリポジトリの\bin\DebugにあるImageToPDF.exeを実行

# Usage

## コマンド
```> ImageToPDF.exe [OPTIONS] [FILES|DIRECTORIES]```  

### FILE
変換元の画像ファイルパスを0個以上指定する。

### DIRECTORIY
変換元の画像ファイルが存在するディレクトリを0個以上指定する。  
このディレクトリ内のすべての画像を変換する。

### Option

- ```-w```, ```--allow-overwrite```  
変換後のファイルと同じ名前のファイルがすでに存在する場合、そのファイルを上書きする。  
何も指定しない場合は上書きされない。

- ```-r```, ```--recursive```  
変換対象にディレクトリを指定した場合に有効。  
そのディレクトリを再帰的に検索し、存在するすべての画像ファイルを変換する。

- ```-t Type```, ```--output-type Type```  
変換後のファイル形式を```Type```に指定する。
    - 何も指定しない場合、```Type```はpdfになる。
    - 各変換元のファイル形式に対して利用可能な```Type```は以下の表のとおり。
    - 変換が不可能な組み合わせ (bmpをpdfに、など)は無視される。

<center>

|  変換元のファイル形式  |  Available ```Type```s  |
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
Microsoft Powerpointのファイル (*.ppt, *.pptx)を変換する場合のみ有効。  
スライドの```start```ページめから```end```ページめまでの各ページを個別の画像ファイルに変換する。  
    - 変換結果のファイル名は、${変換元のファイル名}${ページ番号}${拡張子}となる。  
例えば「ImageToPDF.exe -p 5 7 presentation.pptx」とコマンドを実行した場合、「presentation5.pdf」「presentation6.pdf」「presentation7.pdf」がそれぞれ出力される。
    - 何も指定しない場合、スライドの全ページが変換対象となる。

- ```-v```, ```--version```  
プログラムのバージョン情報を出力して、プログラムを終了する。

- ```-L LogLevel```, ```--log-level LogLevel```  
プログラム実行中に表示するログの最低レベルを```LogLevel```に指定する。  
利用可能な```LogLevel```は```debug```、```info```、```warn```、```error```、```none```の5つ。  
何も指定しない場合、```LogLevel```は```Info```とななる。  
もし変換中にエラーなどが発生した場合、このオプションを```Debug```に指定して出力を確認すると解決法が分かるかもしれません。

## 設定
ImageToPDF.exeと同ディレクトリにsettings.txtを保存することで、設定をカスタマイズできる。

- EpsConverterPath  
ghostscriptの実行ファイルパスを、「EpsConverterPath=$path」の形式で指定する。  
例:  
EpsConverterPath=C:\Program Files\gs\gs9.27\bin\gswin64.exe

- EmfConverterPath  
Metafile to EPS Converterの実行ファイルパスを、「EmfConverterPath=$path」の形式で指定する。  
例:  
EmfConverterPath=C:\Program Files (x86)\Metafile to EPS Converter\metafile2eps.exe

## 既知の不具合
emf/eps/ppt/pptxをpdf/eps/emfに変換する場合、<u>変換対象のファイルへのフルパスが空白を含んでいる</u>と処理が途中で停止してしまう。

# License
MIT

# Author
[Amelia10007] https://github.com/Amelia10007
