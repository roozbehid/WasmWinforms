#include <emscripten.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdint.h>

#include <mono/metadata/assembly.h>
#include <mono/mini/jit.h>
#include <mono/utils/mono-logger.h>
#include <mono/utils/mono-embed.h>
#include <mono/utils/mono-dl-fallback.h>


#include <windows.h>
#include <GdiPlusFlat.h>

//JS funcs
extern MonoObject* mono_wasm_invoke_js_with_args (int js_handle, MonoString *method, MonoArray *args, int *is_exception);
extern MonoObject* mono_wasm_get_object_property (int js_handle, MonoString *method, int *is_exception);
extern MonoObject* mono_wasm_set_object_property (int js_handle, MonoString *method, MonoObject *value, int createIfNotExist, int hasOwnProperty, int *is_exception);
extern MonoObject* mono_wasm_get_global_object (MonoString *globalName, int *is_exception);

// Blazor specific custom routines - see dotnet_support.js for backing code
extern void* mono_wasm_invoke_js_marshalled (MonoString **exceptionMessage, void *asyncHandleLongPtr, MonoString *funcName, MonoString *argsJson);
extern void* mono_wasm_invoke_js_unmarshalled (MonoString **exceptionMessage, MonoString *funcName, void* arg0, void* arg1, void* arg2);

void mono_wasm_enable_debugging (void);

void mono_ee_interp_init (const char *opts);
void mono_marshal_ilgen_init (void);
void mono_method_builder_ilgen_init (void);
void mono_sgen_mono_ilgen_init (void);
void mono_icall_table_init (void);
void mono_aot_register_module (void **aot_info);
char *monoeg_g_getenv(const char *variable);
int monoeg_g_setenv(const char *variable, const char *value, int overwrite);
void mono_free (void*);

int mono_regression_test_step (int verbose_level, char *image, char *method_name);
void mono_trace_init (void);

typedef struct _MonoStringBuilder MonoStringBuilder;
MonoStringBuilder *mono_string_utf16_to_builder2 (const gunichar2 *text);

static char*
m_strdup (const char *str)
{
	if (!str)
		return NULL;

	int len = strlen (str) + 1;
	char *res = malloc (len);
	memcpy (res, str, len);
	return res;
}

static MonoDomain *root_domain;

static MonoString*
mono_wasm_invoke_js (MonoString *str, int *is_exception)
{
	if (str == NULL)
		return NULL;

	char *native_val = mono_string_to_utf8 (str);
	mono_unichar2 *native_res = (mono_unichar2*)EM_ASM_INT ({
		var str = UTF8ToString ($0);
		try {
			var res = eval (str);
			if (res === null || res == undefined)
				return 0;
			res = res.toString ();
			setValue ($1, 0, "i32");
		} catch (e) {
			res = e.toString ();
			setValue ($1, 1, "i32");
			if (res === null || res === undefined)
				res = "unknown exception";
		}
		var buff = Module._malloc((res.length + 1) * 2);
		stringToUTF16 (res, buff, (res.length + 1) * 2);
		return buff;
	}, (int)native_val, is_exception);

	mono_free (native_val);

	if (native_res == NULL)
		return NULL;

	MonoString *res = mono_string_from_utf16 (native_res);
	free (native_res);
	return res;
}

static void
wasm_logger (const char *log_domain, const char *log_level, const char *message, mono_bool fatal, void *user_data)
{
	if (fatal) {
		EM_ASM(
			   var err = new Error();
			   console.log ("Stacktrace: \n");
			   console.log (err.stack);
			   );

		fprintf (stderr, "%s", message);

		abort ();
	} else {
		fprintf (stdout, "%s\n", message);
	}
}

#ifdef ENABLE_AOT
#include "driver-gen.c"
#endif

typedef struct WasmAssembly_ WasmAssembly;

struct WasmAssembly_ {
	MonoBundledAssembly assembly;
	WasmAssembly *next;
};

static WasmAssembly *assemblies;
static int assembly_count;

EMSCRIPTEN_KEEPALIVE void
mono_wasm_add_assembly (const char *name, const unsigned char *data, unsigned int size)
{
	int len = strlen (name);
	if (!strcasecmp (".pdb", &name [len - 4])) {
		char *new_name = m_strdup (name);
		//FIXME handle debugging assemblies with .exe extension
		strcpy (&new_name [len - 3], "dll");
		mono_register_symfile_for_assembly (new_name, data, size);
		return;
	}
	WasmAssembly *entry = (WasmAssembly *)malloc(sizeof (MonoBundledAssembly));
	entry->assembly.name = m_strdup (name);
	entry->assembly.data = data;
	entry->assembly.size = size;
	entry->next = assemblies;
	assemblies = entry;
	++assembly_count;
}

EMSCRIPTEN_KEEPALIVE void
mono_wasm_setenv (const char *name, const char *value)
{
	monoeg_g_setenv (strdup (name), strdup (value), 1);
}

extern int WINAPI invoke_WinMain_Start(int ac, char **av);

int WINAPI WinMain_Start(int ac, char **av){
	//printf("hi from inside..");
	return invoke_WinMain_Start(ac,av);
}

#pragma pack(1)
typedef struct {
	DWORD dwExStyle;
	MonoString *lpClassName;
	MonoString *lpWindowName;
	DWORD dwStyle;
	int x;
	int y;
	int nWidth;
	int nHeight;
	HWND hwndParent;
	HMENU hMenu;
	HINSTANCE hInstance;
	LPVOID lpParam;
} WinCreateClass;
#pragma pack()

/*HWND WINAPI Win32CreateWindowEx(WinCreateClass *wincreate)
{
	HWND result;
	result = CreateWindowEx(wincreate->dwExStyle,mono_string_to_utf8 ((MonoString*)wincreate->lpClassName),mono_string_to_utf8 ((MonoString*)wincreate->lpWindowName),wincreate->dwStyle,wincreate->x,wincreate->y,
	wincreate->nWidth, wincreate->nHeight, wincreate->hwndParent, wincreate->hMenu, wincreate->hInstance, wincreate->lpParam);
    printf ("Win32CreateWindowEx classname : %s Windowname: %s \n", mono_string_to_utf8 ((MonoString*)wincreate->lpClassName),  mono_string_to_utf8 ((MonoString*)wincreate->lpWindowName));
    return result;
}*/
    
EMSCRIPTEN_KEEPALIVE int mono_unbox_int (MonoObject *obj);	
LRESULT CALLBACK WndProc(HWND hwnd, UINT msg, WPARAM wp, LPARAM lp)
{
    void *args [4];
    int val1 = hwnd;
    int val2 = msg;
    int val3 = wp;
    int val4 = lp;
	args [0] = &val1;
    args [1] = &val2;
    args [2] = &val3;
    args [3] = &val4;

	WNDPROC prevregiwndclass = GetWindowLong(hwnd,GWL_WNDPROCBRIDGE);
	int result = 0;

	if (prevregiwndclass != NULL)
		{
			//printf("Running C# delegate for WndProc hwnd=%d msg=%d \n",hwnd,msg);
			MonoObject* resultObject = mono_runtime_delegate_invoke (prevregiwndclass, args, NULL);
			result = mono_unbox_int(resultObject);
			//printf("result of Running C# delegate for WndProc (hwnd=%d msg=%d) : %d \n",hwnd,msg, result);
		}

	return result;
}


ATOM WINAPI WasmRegisterClass(WNDCLASS *lpWndClass)
{
	WNDCLASS newclass = *lpWndClass;
	newclass.lpszClassName =  mono_string_to_utf8 ((MonoString*)lpWndClass->lpszClassName);
	newclass.lpszMenuName =  mono_string_to_utf8 ((MonoString*)lpWndClass->lpszMenuName);
	newclass.lpfnWndProc = WndProc;
	newclass.lpfnWndProcBridge = lpWndClass->lpfnWndProc;
	
	return RegisterClass(&newclass);

}

LONG WINAPI WasmSetWindowLong(HWND hwnd, int nIndex, LONG lNewLong)
{
	SetWindowLong(hwnd,nIndex,lNewLong);
	if (nIndex == GWL_WNDPROC){
		//printf("Win32SetWindowLong with GWL_WNDPROC from managed \n");
		SetWindowLong(hwnd,nIndex,WndProc);
		SetWindowLong(hwnd,GWL_WNDPROCBRIDGE,lNewLong);
	}
}


extern HWND GetAncestor(HWND hwnd, UINT type);

struct dll_list_node {
   char *dll_name;
   MonoDlMapping *dll_mapping;
   struct dll_list_node *next;
};

struct dll_list_node *dll_list = NULL;

	/*mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32CreateWindowEx", Win32CreateWindowEx);
    mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::invoke_WinMain_Start", WinMain_Start);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetSystemMetrics", Win32GetSystemMetrics);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32AdjustWindowRectEx", Win32AdjustWindowRectEx);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32RegisterClass", Win32RegisterClass);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32SetWindowLong", Win32SetWindowLong);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32EnableWindow", Win32EnableWindow);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetAncestor", Win32GetAncestor);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetDesktopWindow", Win32GetDesktopWindow);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetWindowRect", Win32GetWindowRect);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetClientRect", Win32GetClientRect);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32ScreenToClient", Win32ScreenToClient);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32ClientToScreen", Win32ClientToScreen);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::Win32GetParent", Win32GetParent);*/
extern HWND SetParent(HWND hwnd, HWND parent);
extern BOOL WINAPI	SetLayeredWindowAttributes(HWND hwnd, COLORREF crKey,BYTE bAlpha, DWORD dwFlags);
extern BOOL WINAPI GetLayeredWindowAttributes(HWND     hwnd, COLORREF *pcrKey, BYTE     *pbAlpha, DWORD    *pdwFlags);

MonoDlMapping nanox_library_mappings [] = {
	{ "CreateWindowEx", CreateWindowEx },
	{ "invoke_WinMain_Start", invoke_WinMain_Start },
	{ "GetSystemMetrics", GetSystemMetrics },
	{ "AdjustWindowRectEx", AdjustWindowRectEx },
	{ "EnableWindow", EnableWindow },
	{ "GetAncestor", GetAncestor },
	{ "GetDesktopWindow", GetDesktopWindow },
	{ "GetWindowRect", GetWindowRect },
	{ "GetClientRect", GetClientRect },
	{ "ScreenToClient", ScreenToClient },
	{ "ClientToScreen", ClientToScreen },
	{ "GetParent", GetParent },
	{ "GetDC", GetDC },
	{ "ReleaseDC", ReleaseDC },
	{ "SelectObject", SelectObject },
	{ "DeleteObject", DeleteObject },	
	{ "BitBlt", BitBlt },	
	{ "GetSysColor", GetSysColor },	
	{ "CreateFontIndirect", CreateFontIndirect },
	{ "CreateFontIndirectA", CreateFontIndirect },
	{ "CreateFontIndirectW", CreateFontIndirectW },		
	{ "DrawTextA", DrawTextA },	
	{ "SetTextColor", SetTextColor },	
	{ "SetBkColor", SetBkColor },	
	{ "SetBkMode", SetBkMode },	
	{ "SelectClipRgn", SelectClipRgn },	
	{ "MoveWindow", MoveWindow },										
	{ "SetWindowPos", SetWindowPos },		
	{ "DispatchMessage", DispatchMessage },		
	{ "TranslateMessage", TranslateMessage },		
	{ "GetMessage", GetMessage },		
	{ "PeekMessage", PeekMessage },		
	{ "DestroyWindow", DestroyWindow },								
	{ "GetLastError", GetLastError },								
	{ "SetWindowText", SetWindowText },								
	{ "GetWindowText", GetWindowText },								
	{ "SetParent", SetParent },								
	{ "DefWindowProc", DefWindowProc },														
	{ "PostQuitMessage", PostQuitMessage },														
	{ "UpdateWindow", UpdateWindow },														
	{ "GetUpdateRect", GetUpdateRect },														
	{ "BeginPaint", BeginPaint },														
	{ "ValidateRect", ValidateRect },														
	{ "EndPaint", EndPaint },														
	{ "GetWindowDC", GetWindowDC },														
	{ "InvalidateRect", InvalidateRect },																							
	{ "SetActiveWindow", SetActiveWindow },																							
	{ "CreateSolidBrush", CreateSolidBrush },																							
	{ "ShowWindow", ShowWindow  },							
	{ "GetWindowLong", GetWindowLong},					
	{ "GetFocus", GetFocus },						
	{ "SetFocus", SetFocus },							
	{ "SetTimer", SetTimer },							
	{ "KillTimer", KillTimer },									
	{ "SendMessageA", SendMessage },
	{ "SendMessageW", SendMessageW },			
	{ "PostMessageW", PostMessage },				
	{ "GetActiveWindow", GetActiveWindow },
	{ "PostMessageA", PostMessage },
	{ "SetLayeredWindowAttributes", SetLayeredWindowAttributes },
	{ "GetLayeredWindowAttributes", GetLayeredWindowAttributes },
	{ "SetCapture", SetCapture },			
	{ "GetCapture", GetCapture },
	{ "ReleaseCapture", ReleaseCapture },
	{ "SystemParametersInfoA", SystemParametersInfoA },		
	{ "SystemParametersInfoW", SystemParametersInfoW },	
	{ "CreateCaret", CreateCaret },	
	{ "SetCaretPos", SetCaretPos },	
	{ "GetCaretPos", GetCaretPos },	
	{ "ShowCaret", ShowCaret },	
	{ "HideCaret", HideCaret },	
	{ "DestroyCaret", DestroyCaret },	
	{ "GetCursorPos", GetCursorPos },	
    { NULL, NULL }
};

  /*mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipSetStringFormatLineAlign", Win32GdipSetStringFormatLineAlign);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdiplusStartup", Win32GdiplusStartup);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipCreateStringFormat", Win32GdipCreateStringFormat);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipSetStringFormatAlign", Win32GdipSetStringFormatAlign);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipSetStringFormatHotkeyPrefix", Win32GdipSetStringFormatHotkeyPrefix);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetStringFormatFlags", Win32GdipGetStringFormatFlags);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetGenericFontFamilySansSerif", Win32GdipGetGenericFontFamilySansSerif);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetFamilyName", Win32GdipGetFamilyName);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipCreateFont", Win32GdipCreateFont);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipCreateFromHWND", Win32GdipCreateFromHWND);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipCreateBitmapFromScan0", Win32GdipCreateBitmapFromScan0);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetImagePixelFormat", Win32GdipGetImagePixelFormat);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetImageGraphicsContext", Win32GdipGetImageGraphicsContext);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetDpiX", Win32GdipGetDpiX);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetDpiY", Win32GdipGetDpiY);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetFontHeightGivenDPI", Win32GdipGetFontHeightGivenDPI);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetFontHeight", Win32GdipGetFontHeight);
	mono_add_internal_call ("System.Drawing.GDIPlus::Win32GdipGetDC", Win32GdipGetDC);*/

MonoDlMapping libgdi_library_mappings [] = {
	{ "GdipSetStringFormatLineAlign", GdipSetStringFormatLineAlign },
	{ "GdiplusStartup", GdiplusStartup },
	{ "GdipCreateStringFormat", GdipCreateStringFormat },
	{ "GdipSetStringFormatAlign", GdipSetStringFormatAlign },
	{ "GdipSetStringFormatHotkeyPrefix", GdipSetStringFormatHotkeyPrefix },
	{ "GdipGetStringFormatFlags", GdipGetStringFormatFlags },
	{ "GdipGetGenericFontFamilySansSerif", GdipGetGenericFontFamilySansSerif },
	{ "GdipGetFamilyName", GdipGetFamilyName },
	{ "GdipCreateFont", GdipCreateFont },
	{ "GdipCreateFromHWND", GdipCreateFromHWND },
	{ "GdipCreateBitmapFromScan0", GdipCreateBitmapFromScan0 },
	{ "GdipGetImagePixelFormat", GdipGetImagePixelFormat },
	{ "GdipGetImageGraphicsContext", GdipGetImageGraphicsContext },
	{ "GdipGetDpiX", GdipGetDpiX },
	{ "GdipGetDpiY", GdipGetDpiY },
	{ "GdipGetFontHeightGivenDPI", GdipGetFontHeightGivenDPI },
	{ "GdipGetFontHeight", GdipGetFontHeight },
	{ "GdipSetStringFormatFlags", GdipSetStringFormatFlags },
	{ "GdipSetStringFormatTrimming", GdipSetStringFormatTrimming },
	{ "GdipGetStringFormatTrimming", GdipGetStringFormatTrimming },
	{ "GdipGetDC", GdipGetDC },	
	{ "GdipReleaseDC", GdipReleaseDC },	
	{ "GdipDrawString", GdipDrawString },	
	{ "GdipFillRectangles", GdipFillRectangles },	
	{ "GdipCreateFromHDC", GdipCreateFromHDC },	
	{ "GdipDeleteGraphics", GdipDeleteGraphics },					
	{ "GdipRestoreGraphics", GdipRestoreGraphics },					
	{ "GdipGetLogFontA", GdipGetLogFontA },					
	{ "GdipGetLogFontW", GdipGetLogFontW },		
	{ "GdipDeleteFont", GdipDeleteFont },					
	{ "CreateFontIndirect", CreateFontIndirect },									
	{ "GdipCreateFontFromLogfontA", GdipCreateFontFromLogfontA },									
	{ "GdipCreateFontFromLogfontW", GdipCreateFontFromLogfontW },
	{ "GdipCreateSolidFill", GdipCreateSolidFill },
	{ "GdipFillRectangleI", GdipFillRectangleI },	
	{ "GdipSetClipRectI", GdipSetClipRectI },	
	/**/{ "GdipCreatePen1", GdipCreatePen1 },	
	{ "GdipDrawLineI", GdipDrawLineI },	
	{ "GdipDrawLine", GdipDrawLine },	
	{ "GdipDrawRectangleI", GdipDrawRectangleI },	
	{ "GdipDrawRectangle", GdipDrawRectangle },	
	{ "GdipDrawPolygon", GdipDrawPolygon },	
	{ "GdipDrawPolygonI", GdipDrawPolygonI },	
	{ "GdipCreateMatrix", GdipCreateMatrix },										
	{ "GdipCreateMatrix2", GdipCreateMatrix2 },										
	{ "GdipCreateMatrix3", GdipCreateMatrix3 },										
	{ "GdipCreateMatrix3I", GdipCreateMatrix3I },										
	{ "GdipDeleteMatrix", GdipDeleteMatrix },										
	{ "GdipCloneMatrix", GdipCloneMatrix },										
	{ "GdipSetMatrixElements", GdipSetMatrixElements },										
	{ "GdipGetMatrixElements", GdipGetMatrixElements },										
	{ "GdipTranslateMatrix", GdipTranslateMatrix },										
	{ "GdipScaleMatrix", GdipScaleMatrix },										
	{ "GdipRotateMatrix", GdipRotateMatrix },										
	{ "GdipIsMatrixEqual", GdipIsMatrixEqual },										
	{ "GdipIsMatrixIdentity", GdipIsMatrixIdentity },																						
	{ "GdipIsMatrixInvertible", GdipIsMatrixInvertible },																						
	{ "GdipVectorTransformMatrixPoints", GdipVectorTransformMatrixPoints },																						
	{ "GdipTransformMatrixPointsI", GdipTransformMatrixPointsI },																						
	{ "GdipTransformMatrixPoints", GdipTransformMatrixPoints },																						
	{ "GdipInvertMatrix", GdipInvertMatrix },																										
	{ "GdipGetWorldTransform", GdipGetWorldTransform },																										
	{ "GdipSetWorldTransform", GdipSetWorldTransform },																										
	{ "GdipResetWorldTransform", GdipResetWorldTransform },																										
	{ "GdipScaleWorldTransform", GdipScaleWorldTransform },																										
	{ "GdipCreateHatchBrush", GdipCreateHatchBrush },																											
	{ "GdipCreatePen2", GdipCreatePen2 },																															
	{ "GdipDeletePen", GdipDeletePen },	
	{ "GdipClonePen", GdipClonePen },	
	{ "GdipSetPenBrushFill", GdipSetPenBrushFill },	
	{ "GdipGetPenFillType", GdipGetPenFillType },			
	{ "GdipSetPenColor", GdipSetPenColor },			
	{ "GdipGetPenColor", GdipGetPenColor },			
	{ "GdipSetPenWidth", GdipSetPenWidth },			
	{ "GdipGetPenWidth", GdipGetPenWidth },			
	{ "GdipSetPenMode", GdipSetPenMode },			
	{ "GdipGetPenMode", GdipGetPenMode },	
	{ "GdipSetPenTransform", GdipSetPenTransform },			
	{ "GdipGetPenTransform", GdipGetPenTransform },	
	{ "GdipStringFormatGetGenericTypographic", GdipStringFormatGetGenericTypographic },			
	{ "GdipCloneStringFormat", GdipCloneStringFormat },	
	{ "GdipMeasureString", GdipMeasureString },			
	{ "GdipSetStringFormatMeasurableCharacterRanges", GdipSetStringFormatMeasurableCharacterRanges },	
	{ "GdipGetStringFormatMeasurableCharacterRangeCount", GdipGetStringFormatMeasurableCharacterRangeCount },			
	{ "GdipCreateRegion", GdipCreateRegion },									
	{ "GdipCreateRegionRect", GdipCreateRegionRect },	
	{ "GdipCreateRegionRectI", GdipCreateRegionRectI },	
	{ "GdipCreateRegionPath", GdipCreateRegionPath },	
	{ "GdipCreateRegionRgnData", GdipCreateRegionRgnData },	
	{ "GdipCloneRegion", GdipCloneRegion },	
	{ "GdipDeleteRegion", GdipDeleteRegion },	
	{ "GdipCombineRegionRect", GdipCombineRegionRect },	
	{ "GdipGetRegionBounds", GdipGetRegionBounds },	
	{ "GdipMeasureCharacterRanges", GdipMeasureCharacterRanges },	
	{ "GdipFillPolygon2", GdipFillPolygon2 },	
	{ "GdipCreateFontFamilyFromName",  GdipCreateFontFamilyFromName },	
	{ "GdipGetGenericFontFamilySerif", GdipGetGenericFontFamilySerif },	
	{ "GdipGetGenericFontFamilyMonospace", GdipGetGenericFontFamilyMonospace },	
	{ "GdipFlush", GdipFlush },	
	{ "GdipFillPolygonI", GdipFillPolygonI },	
	{ "GdipDeleteStringFormat", GdipDeleteStringFormat },	
	{ "GdipGetClip", GdipGetClip },	
	{ "GdipSetClipRegion", GdipSetClipRegion },	
	{ "GdipFillEllipseI", GdipFillEllipseI },	
	{ "GdipFillPie", GdipFillPie },	
	{ "GdipDrawArc", GdipDrawArc },	
	{ "GdipDrawArcI", GdipDrawArcI },	
	{ "GdipCombineRegionRectI", GdipCombineRegionRectI },	
	{ "GdipIsVisibleRegionRectI", GdipIsVisibleRegionRectI },	
	{ "GdipIsVisibleRegionRect", GdipIsVisibleRegionRect },	
	{ "GdipIsVisibleRegionPointI", GdipIsVisibleRegionPointI },	
	{ "GdipIsVisibleRegionPoint", GdipIsVisibleRegionPoint },	
	{ "GdipCreateLineBrushFromRectI", GdipCreateLineBrushFromRectI },	
	{ "GdipDeleteBrush", GdipDeleteBrush },	
	{ "GdipTranslateWorldTransform", GdipTranslateWorldTransform },	
	{ "GdipRotateWorldTransform", GdipRotateWorldTransform },	
	{ "GdipMultiplyWorldTransform", GdipMultiplyWorldTransform },	
	{ "GdipGraphicsClear", GdipGraphicsClear },	
	{ "GdipDrawLinesI", GdipDrawLinesI },	
	{ "GdipFillPie", GdipFillPie },	
	{ "GdipFillPie", GdipFillPie },	
    { NULL, NULL }
};

static void *
dl_mapping_open (const char *file, int flags, char **err, void *user_data)
{
	printf("dl_mapping_open %s -",file);
	MonoDlMapping *mappings;
	
	struct dll_list_node* current = dll_list;
	if (current == NULL)
		{
			printf(" dll_list were empty \n");
			return NULL;
		}

	while (strcmp(file,current->dll_name) != 0)
	{
		current = current->next;
		if (current == NULL)
			{
				printf("%s could not be found in my dl_mapping \n",file);
				return NULL;
			}
	}

	printf("%s matched and found!\n", file);
	mappings = current->dll_mapping;

	*err = g_strdup (mappings == NULL ? "File not registered" : "");
	return mappings;
}

static void *
dl_mapping_symbol (void *handle, const char *symbol, char **err, void *user_data)
{
	printf("dl_mapping_symbol %s - ",symbol);
	MonoDlMapping *mappings = (MonoDlMapping *) handle;
	
	for (;mappings->name; mappings++){
		if (strcmp (symbol, mappings->name) == 0){
			*err = g_strdup ("");
			printf("proc name %s matched\n",symbol);
			return mappings->addr;
		}
	}
	printf("proc name %s not matched\n", symbol);
	*err = g_strdup ("Symbol not found");
	return NULL;
}


void
mono_dl_register_library (const char *name, MonoDlMapping *mappings)
{
	printf("mono_dl_register_library %s\n",name);
	struct dll_list_node *node;

	node = malloc(sizeof(struct dll_list_node));
	node->dll_name = strdup (name);
	node->dll_mapping = mappings;

	if (dll_list == NULL){
		dll_list = node;
		mono_dl_fallback_register (dl_mapping_open, dl_mapping_symbol, NULL, NULL);
		return ;
	}

	struct dll_list_node* current = dll_list;
	while (current)
	{
		if (!current->next){
			current->next = node;
			return;
		}
		current = current->next;
	}
	
}

EMSCRIPTEN_KEEPALIVE void
mono_wasm_load_runtime (const char *managed_path, int enable_debugging)
{
	monoeg_g_setenv ("MONO_LOG_LEVEL", "debug", 0);
	monoeg_g_setenv ("MONO_LOG_MASK", "gc", 0);

#ifdef ENABLE_AOT
	// Defined in driver-gen.c
	register_aot_modules ();
	mono_jit_set_aot_mode (MONO_AOT_MODE_LLVMONLY);
#else
	mono_jit_set_aot_mode (MONO_AOT_MODE_INTERP_LLVMONLY);
	if (enable_debugging)
		mono_wasm_enable_debugging ();
#endif

#ifndef ENABLE_AOT
	mono_icall_table_init ();
	mono_ee_interp_init ("");
	mono_marshal_ilgen_init ();
	mono_method_builder_ilgen_init ();
	mono_sgen_mono_ilgen_init ();
#endif

	if (assembly_count) {
		MonoBundledAssembly **bundle_array = (MonoBundledAssembly **)calloc (1, sizeof (MonoBundledAssembly*) * (assembly_count + 1));
		WasmAssembly *cur = assemblies;
		bundle_array [assembly_count] = NULL;
		int i = 0;
		while (cur) {
			bundle_array [i] = &cur->assembly;
			cur = cur->next;
			++i;
		}
		mono_register_bundled_assemblies ((const MonoBundledAssembly**)bundle_array);
	}

	mono_trace_init ();
	mono_trace_set_log_handler (wasm_logger, NULL);
	root_domain = mono_jit_init_version ("mono", "v4.0.30319");

	mono_add_internal_call ("WebAssembly.Runtime::InvokeJS", mono_wasm_invoke_js);
	mono_add_internal_call ("WebAssembly.Runtime::InvokeJSWithArgs", mono_wasm_invoke_js_with_args);
	mono_add_internal_call ("WebAssembly.Runtime::GetObjectProperty", mono_wasm_get_object_property);
	mono_add_internal_call ("WebAssembly.Runtime::SetObjectProperty", mono_wasm_set_object_property);
	mono_add_internal_call ("WebAssembly.Runtime::GetGlobalObject", mono_wasm_get_global_object);

	// Blazor specific custom routines - see dotnet_support.js for backing code		
	mono_add_internal_call ("WebAssembly.JSInterop.InternalCalls::InvokeJSMarshalled", mono_wasm_invoke_js_marshalled);
	mono_add_internal_call ("WebAssembly.JSInterop.InternalCalls::InvokeJSUnmarshalled", mono_wasm_invoke_js_unmarshalled);

	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::WasmRegisterClass", WasmRegisterClass);
	mono_add_internal_call ("System.Windows.Forms.XplatUINanoX::WasmSetWindowLong", WasmSetWindowLong);

	mono_dl_register_library ("libnanox.dll", nanox_library_mappings);
	mono_dl_register_library ("libgdiplus.dll", libgdi_library_mappings);
	
	
    setenv("FONTCONFIG_PATH","/etc", 1);

}

EMSCRIPTEN_KEEPALIVE MonoAssembly*
mono_wasm_assembly_load (const char *name)
{
	MonoImageOpenStatus status;
	MonoAssemblyName* aname = mono_assembly_name_new (name);
	if (!name)
		return NULL;

	MonoAssembly *res = mono_assembly_load (aname, NULL, &status);
	mono_assembly_name_free (aname);

	return res;
}

EMSCRIPTEN_KEEPALIVE MonoClass*
mono_wasm_assembly_find_class (MonoAssembly *assembly, const char *namespace, const char *name)
{
	return mono_class_from_name (mono_assembly_get_image (assembly), namespace, name);
}

EMSCRIPTEN_KEEPALIVE MonoMethod*
mono_wasm_assembly_find_method (MonoClass *klass, const char *name, int arguments)
{
	return mono_class_get_method_from_name (klass, name, arguments);
}

EMSCRIPTEN_KEEPALIVE MonoObject*
mono_wasm_invoke_method (MonoMethod *method, MonoObject *this_arg, void *params[], int* got_exception)
{
	MonoObject *exc = NULL;
	MonoObject *res = mono_runtime_invoke (method, this_arg, params, &exc);
	*got_exception = 0;

	if (exc) {
		*got_exception = 1;

		MonoObject *exc2 = NULL;
		res = (MonoObject*)mono_object_to_string (exc, &exc2); 
		if (exc2)
			res = (MonoObject*) mono_string_new (root_domain, "Exception Double Fault");
		return res;
	}

	return res;
}

EMSCRIPTEN_KEEPALIVE MonoMethod*
mono_wasm_assembly_get_entry_point (MonoAssembly *assembly)
{
	MonoImage *image;
	MonoMethod *method;

	image = mono_assembly_get_image (assembly);
	uint32_t entry = mono_image_get_entry_point (image);
	if (!entry)
		return NULL;

	return mono_get_method (image, entry, NULL);
}

EMSCRIPTEN_KEEPALIVE char *
mono_wasm_string_get_utf8 (MonoString *str)
{
	return mono_string_to_utf8 (str); //XXX JS is responsible for freeing this
}

EMSCRIPTEN_KEEPALIVE MonoString *
mono_wasm_string_from_js (const char *str)
{
	return mono_string_new (root_domain, str);
}

static int
class_is_task (MonoClass *klass)
{
	if (!strcmp ("System.Threading.Tasks", mono_class_get_namespace (klass)) && 
		(!strcmp ("Task", mono_class_get_name (klass)) || !strcmp ("Task`1", mono_class_get_name (klass))))
		return 1;

	return 0;
}

#define MARSHAL_TYPE_INT 1
#define MARSHAL_TYPE_FP 2
#define MARSHAL_TYPE_STRING 3
#define MARSHAL_TYPE_VT 4
#define MARSHAL_TYPE_DELEGATE 5
#define MARSHAL_TYPE_TASK 6
#define MARSHAL_TYPE_OBJECT 7
#define MARSHAL_TYPE_BOOL 8
#define MARSHAL_TYPE_ENUM 9

// typed array marshalling
#define MARSHAL_ARRAY_BYTE 11
#define MARSHAL_ARRAY_UBYTE 12
#define MARSHAL_ARRAY_SHORT 13
#define MARSHAL_ARRAY_USHORT 14
#define MARSHAL_ARRAY_INT 15
#define MARSHAL_ARRAY_UINT 16
#define MARSHAL_ARRAY_FLOAT 17
#define MARSHAL_ARRAY_DOUBLE 18

EMSCRIPTEN_KEEPALIVE int
mono_wasm_get_obj_type (MonoObject *obj)
{
	if (!obj)
		return 0;
	MonoClass *klass = mono_object_get_class (obj);
	MonoType *type = mono_class_get_type (klass);

	switch (mono_type_get_type (type)) {
	// case MONO_TYPE_CHAR: prob should be done not as a number?
	case MONO_TYPE_BOOLEAN:
		return MARSHAL_TYPE_BOOL;
	case MONO_TYPE_I1:
	case MONO_TYPE_U1:
	case MONO_TYPE_I2:
	case MONO_TYPE_U2:
	case MONO_TYPE_I4:
	case MONO_TYPE_U4:
	case MONO_TYPE_I8:
	case MONO_TYPE_U8:
		return MARSHAL_TYPE_INT;
	case MONO_TYPE_R4:
	case MONO_TYPE_R8:
		return MARSHAL_TYPE_FP;
	case MONO_TYPE_STRING:
		return MARSHAL_TYPE_STRING;
	case MONO_TYPE_SZARRAY:  { // simple zero based one-dim-array
		MonoClass *eklass = mono_class_get_element_class(klass);
		MonoType *etype = mono_class_get_type (eklass);

		switch (mono_type_get_type (etype)) {
			case MONO_TYPE_U1:
				return MARSHAL_ARRAY_UBYTE;
			case MONO_TYPE_I1:
				return MARSHAL_ARRAY_BYTE;
			case MONO_TYPE_U2:
				return MARSHAL_ARRAY_USHORT;			
			case MONO_TYPE_I2:
				return MARSHAL_ARRAY_SHORT;			
			case MONO_TYPE_U4:
				return MARSHAL_ARRAY_UINT;			
			case MONO_TYPE_I4:
				return MARSHAL_ARRAY_INT;			
			case MONO_TYPE_R4:
				return MARSHAL_ARRAY_FLOAT;
			case MONO_TYPE_R8:
				return MARSHAL_ARRAY_DOUBLE;
			default:
				return MARSHAL_TYPE_OBJECT;
		}		
	}
	default:
		if (mono_class_is_enum (klass))
			return MARSHAL_TYPE_ENUM;
		if (!mono_type_is_reference (type)) //vt
			return MARSHAL_TYPE_VT;
		if (mono_class_is_delegate (klass))
			return MARSHAL_TYPE_DELEGATE;
		if (class_is_task(klass))
			return MARSHAL_TYPE_TASK;

		return MARSHAL_TYPE_OBJECT;
	}
}


EMSCRIPTEN_KEEPALIVE int
mono_unbox_int (MonoObject *obj)
{
	if (!obj)
		return 0;
	MonoType *type = mono_class_get_type (mono_object_get_class(obj));

	void *ptr = mono_object_unbox (obj);
	switch (mono_type_get_type (type)) {
	case MONO_TYPE_I1:
	case MONO_TYPE_BOOLEAN:
		return *(signed char*)ptr;
	case MONO_TYPE_U1:
		return *(unsigned char*)ptr;
	case MONO_TYPE_I2:
		return *(short*)ptr;
	case MONO_TYPE_U2:
		return *(unsigned short*)ptr;
	case MONO_TYPE_I4:
		return *(int*)ptr;
	case MONO_TYPE_U4:
		return *(unsigned int*)ptr;
	// WASM doesn't support returning longs to JS
	// case MONO_TYPE_I8:
	// case MONO_TYPE_U8:
	default:
		printf ("Invalid type %d to mono_unbox_int\n", mono_type_get_type (type));
		return 0;
	}
}

EMSCRIPTEN_KEEPALIVE double
mono_wasm_unbox_float (MonoObject *obj)
{
	if (!obj)
		return 0;
	MonoType *type = mono_class_get_type (mono_object_get_class(obj));

	void *ptr = mono_object_unbox (obj);
	switch (mono_type_get_type (type)) {
	case MONO_TYPE_R4:
		return *(float*)ptr;
	case MONO_TYPE_R8:
		return *(double*)ptr;
	default:
		printf ("Invalid type %d to mono_wasm_unbox_float\n", mono_type_get_type (type));
		return 0;
	}
}

EMSCRIPTEN_KEEPALIVE int
mono_wasm_array_length (MonoArray *array)
{
	return mono_array_length (array);
}

EMSCRIPTEN_KEEPALIVE MonoObject*
mono_wasm_array_get (MonoArray *array, int idx)
{
	return mono_array_get (array, MonoObject*, idx);
}

EMSCRIPTEN_KEEPALIVE MonoArray*
mono_wasm_obj_array_new (int size)
{
	return mono_array_new (root_domain, mono_get_object_class (), size);
}

EMSCRIPTEN_KEEPALIVE void
mono_wasm_obj_array_set (MonoArray *array, int idx, MonoObject *obj)
{
	mono_array_setref (array, idx, obj);
}

EMSCRIPTEN_KEEPALIVE MonoArray*
mono_wasm_string_array_new (int size)
{
	return mono_array_new (root_domain, mono_get_string_class (), size);
}

// Int8Array 		| int8_t	| byte or SByte (signed byte)
// Uint8Array		| uint8_t	| byte or Byte (unsigned byte)
// Uint8ClampedArray| uint8_t	| byte or Byte (unsigned byte)
// Int16Array		| int16_t	| short (signed short)
// Uint16Array		| uint16_t	| ushort (unsigned short)
// Int32Array		| int32_t	| int (signed integer)
// Uint32Array		| uint32_t	| uint (unsigned integer)
// Float32Array		| float		| float
// Float64Array		| double	| double

EMSCRIPTEN_KEEPALIVE MonoArray*
mono_wasm_typed_array_new (char *arr, int length, int size, int type)
{
	MonoClass *typeClass = mono_get_byte_class(); // default is Byte
	switch (type) {
	case MARSHAL_ARRAY_BYTE:
		typeClass = mono_get_sbyte_class();
		break;
	case MARSHAL_ARRAY_SHORT:
		typeClass = mono_get_int16_class();
		break;
	case MARSHAL_ARRAY_USHORT:
		typeClass = mono_get_uint16_class();
		break;
	case MARSHAL_ARRAY_INT:
		typeClass = mono_get_int32_class();
		break;
	case MARSHAL_ARRAY_UINT:
		typeClass = mono_get_uint32_class();
		break;
	case MARSHAL_ARRAY_FLOAT:
		typeClass = mono_get_single_class();
		break;
	case MARSHAL_ARRAY_DOUBLE:
		typeClass = mono_get_double_class();
		break;
	}

	MonoArray *buffer;

	buffer = mono_array_new (root_domain, typeClass, length);
	memcpy(mono_array_addr_with_size(buffer, sizeof(char), 0), arr, length * size);

	return buffer;
}


EMSCRIPTEN_KEEPALIVE void
mono_wasm_array_to_heap (MonoArray *src, char *dest)
{
	int element_size;
	void *source_addr;
	int arr_length;

	element_size = mono_array_element_size ( mono_object_get_class((MonoObject*)src));
	//DBG("mono_wasm_to_heap element size %i  / length %i\n",element_size, mono_array_length(src));

	// get our src address
	source_addr = mono_array_addr_with_size (src, element_size, 0);
	// copy the array memory to heap via ptr dest
	memcpy (dest, source_addr, mono_array_length(src) * element_size);
}

EMSCRIPTEN_KEEPALIVE int
mono_wasm_exec_regression (int verbose_level, char *image)
{
	return mono_regression_test_step (verbose_level, image, NULL) ? 0 : 1;
}

EMSCRIPTEN_KEEPALIVE int
mono_wasm_unbox_enum (MonoObject *obj)
{
	if (!obj)
		return 0;
	
	MonoType *type = mono_class_get_type (mono_object_get_class(obj));

	void *ptr = mono_object_unbox (obj);
	switch (mono_type_get_type(mono_type_get_underlying_type (type))) {
	case MONO_TYPE_I1:
	case MONO_TYPE_U1:
		return *(unsigned char*)ptr;
	case MONO_TYPE_I2:
		return *(short*)ptr;
	case MONO_TYPE_U2:
		return *(unsigned short*)ptr;
	case MONO_TYPE_I4:
		return *(int*)ptr;
	case MONO_TYPE_U4:
		return *(unsigned int*)ptr;
	// WASM doesn't support returning longs to JS
	// case MONO_TYPE_I8:
	// case MONO_TYPE_U8:
	default:
		printf ("Invalid type %d to mono_unbox_enum\n", mono_type_get_type(mono_type_get_underlying_type (type)));
		return 0;
	}
}

EMSCRIPTEN_KEEPALIVE int
mono_wasm_exit (int exit_code)
{
	exit (exit_code);
}
