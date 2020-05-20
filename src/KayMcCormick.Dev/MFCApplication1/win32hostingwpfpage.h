#pragma once

#include "resource.h"
#include "WPFPage.h"
#include <vcclr.h>
#include <string.h>

public ref class WPFPageHost
{
public:
  WPFPageHost();
  static WPFPage^ hostedPage;
  //initial property settings
  static System::Windows::Media::Brush^ initBackBrush;
  static System::Windows::Media::Brush^ initForeBrush;
  static System::Windows::Media::FontFamily^ initFontFamily;
  static System::Windows::FontStyle initFontStyle;
  static System::Windows::FontWeight initFontWeight;
  static double initFontSize;
};

RECT rect;
HWND GetHwnd(HWND parent, int x, int y, int width, int height);
