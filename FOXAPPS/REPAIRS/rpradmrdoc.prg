   *******************************************************
   * PROGRAM NAME: RPRADMDOC.PRG                        
   *-----------------------------------------------------
   * This routine opens a WORD doc called ADM_METER		 
   * and populates fields  								 
   * Save this as RPRADMDOC.PRG                          
   *******************************************************

*--Word macro modified for Visual FoxPro
#DEFINE True .T.
#DEFINE False .F.

PARAMETERS DOC
LOCAL loWord

Path = ""
loWord = CreateObject("word.application")
DO REPAIRS\RPRPATH.PRG

loWord.Documents.Open (PATH+DOC, .F., .T.)
loWord.Visible = .F.
loWord.Visible = .T.

coid = ""
orderno = ""
serial_no = ""
model_no = ""
company_name=""
city = ""
state=""
date_received=""
lastcal=""
meter = ""
access_kit = ""
access_part = ""
fhkit = ""
fhbase = ""
multitemp = ""
notesfield = ""
pono = ""


select precheck

store co_id to coid
store serial_num to serial_no
store model_num to model_no
store order_no to orderno
store recvd_from to company_name
store city to city
store state to state
store date_recvd to date_received
store last_cal to lastcal
store meter to meter
store acces_kit to access_kit
store acces_part to access_part
store flhd_kit to fhkit
store flhd_base to fhbase
store mltitmp_kt to multitemp
store notes to notesfield
STORE po_no TO pono
NOTETRIM = ""

*PreProcessing
FOR N = 1 TO MEMLINES(notesfield)
	NOTETRIM = NOTETRIM + " " + MLINE(notesfield, N)
NEXT	

IF len(TRIM(company_name)) > 40 
	company_name = SUBSTR(company_name, 1, 40)
ENDIF


If loWord.ActiveDocument.Bookmarks.Exists("v_coid_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid_1").Select
	If Empty(coid)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_______ ")
	Else
		loWord.Selection.TypeText(trim(coid))	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_coid_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid_2").Select
	If Empty(coid)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_______ ")
	Else
		loWord.Selection.TypeText(trim(coid))	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_coid_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid_3").Select
	If Empty(coid)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_______ ")
	Else
		loWord.Selection.TypeText(trim(coid))	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_coid_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid_4").Select
	If Empty(coid)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_______ ")
	Else
		loWord.Selection.TypeText(trim(coid))	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("notes1") = .T.
	loWord.ActiveDocument.Bookmarks("notes1").Select
	notetrim = trim(notetrim)
	If Empty(notetrim)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("___________________________________________________________________________________ ")
	Else
		loWord.Selection.TypeText(notetrim)	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("notes2") = .T.
	loWord.ActiveDocument.Bookmarks("notes2").Select
	notetrim = TRIM(notetrim)
	If Empty(notetrim)
		loWord.Selection.Font.Underline = 0 	
		loWord.Selection.TypeText("___________________________________________________________________________________")
	Else
		loWord.Selection.TypeText(notetrim)	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_city_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_city_1").Select
	If  !EMPTY(city)
		loWord.Selection.TypeText(city)
	Else
		loWord.Selection.TypeText("                      ")
	EndIf	
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_city_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_city_2").Select
	If  !EMPTY(CITY)
		loWord.Selection.TypeText(city)
	Else
		loWord.Selection.TypeText("                      ")
	EndIf	
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_city_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_city_3").Select
	If !EMPTY(city)
		loWord.Selection.TypeText(city)
	Else
		loWord.Selection.TypeText("                      ")
	EndIf	
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_city_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_city_4").Select
	If !EMPTY(city)
		loWord.Selection.TypeText(city)
	Else
		loWord.Selection.TypeText("                      ")
	EndIf	
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_ser_no_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_ser_no_1").Select
	If !EMPTY(serial_no)
		loWord.Selection.TypeText(serial_no)
	Else
		loWord.Selection.TypeText("          ")
	EndIf			
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_mod_no_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_mod_no_1").Select
	If !EMPTY(model_no)
		loWord.Selection.TypeText(model_no)
	Else
		loWord.Selection.TypeText("          ")
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_company_name_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_company_name_1").Select
	loWord.Selection.TypeText(company_name)
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_ordno_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_ordno_1").Select
	loWord.Selection.TypeText(orderno)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_st_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_st_1").Select
	If  !EMPTY(state)
		loWord.Selection.TypeText(state)
	Else
		loWord.Selection.TypeText("       ")
	EndIf	
EndIf		

If loWord.ActiveDocument.Bookmarks.Exists("v_date_rec_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_date_rec_1").Select
	loWord.Selection.TypeText(DTOC(date_received))	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_cal_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_cal_1").Select
	If !EMPTY(DTOC(lastcal))
		loWord.Selection.TypeText(DTOC(lastcal))
	Else
		loWord.Selection.TypeText("        ")
	EndIf			
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("vm1") = .T.
	loWord.ActiveDocument.Bookmarks("vm1").Select
	If !EMPTY(meter)
		loWord.Selection.TypeText(meter)
	else
		loWord.Selection.TypeText("     ")
	Endif			
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ak1") = .T.
	loWord.ActiveDocument.Bookmarks("ak1").Select
	If !EMPTY(access_kit)
		loWord.Selection.TypeText(access_kit)	
	Else
		loWord.Selection.TypeText("     ")
	EndIf	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ap1") = .T.
	loWord.ActiveDocument.Bookmarks("ap1").Select
	If !EMPTY(access_part)
		loWord.Selection.TypeText(access_part)
	Else	
		loWord.Selection.TypeText("     ")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("fh1") = .T.
	loWord.ActiveDocument.Bookmarks("fh1").Select
	If !EMPTY(fhkit) 
		loWord.Selection.TypeText(fhkit)
	Else
		loWord.Selection.TypeText("     ")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ba1") = .T.
	loWord.ActiveDocument.Bookmarks("ba1").Select
	If !EMPTY(fhbase)
		loWord.Selection.TypeText(fhbase)
	Else		
		loWord.Selection.TypeText("     ")
	EndIf	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("tm1") = .T.
	loWord.ActiveDocument.Bookmarks("tm1").Select
	If !EMPTY(multitemp) 
		loWord.Selection.TypeText(multitemp)
	Else
		loWord.Selection.TypeText("___")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_ser_no_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_ser_no_2").Select
	loWord.Selection.TypeText(serial_no)
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_mod_no_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_mod_no_2").Select
	loWord.Selection.TypeText(model_no)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_company_name_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_company_name_2").Select
	loWord.Selection.TypeText(company_name)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_ordno_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_ordno_2").Select
	loWord.Selection.TypeText(orderno)
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_st_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_st_2").Select
	If !EMPTY(state) 
		loWord.Selection.TypeText(state)
	Else
		loWord.Selection.TypeText("       ")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_date_rec_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_date_rec_2").Select
	loWord.Selection.TypeText(DTOC(date_received))
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_cal_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_cal_2").Select
	If !EMPTY(DTOC(lastcal))
		loWord.Selection.TypeText(DTOC(lastcal))
	Else	
		loWord.Selection.TypeText("        ")
	EndIf	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("vm2") = .T.
	loWord.ActiveDocument.Bookmarks("vm2").Select
	If !EMPTY(meter)
		loWord.Selection.TypeText(meter)
	else
		loWord.Selection.TypeText("     ")
	Endif		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ak2") = .T.
	loWord.ActiveDocument.Bookmarks("ak2").Select
	If !EMPTY(access_kit)
		loWord.Selection.TypeText(access_kit)	
	Else
		loWord.Selection.TypeText("     ")
	EndIf	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ap2") = .T.
	loWord.ActiveDocument.Bookmarks("ap2").Select
	If !EMPTY(access_part)
		loWord.Selection.TypeText(access_part)
	Else	
		loWord.Selection.TypeText("     ")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("fh2") = .T.
	loWord.ActiveDocument.Bookmarks("fh2").Select
	If !EMPTY(fhkit) 
		loWord.Selection.TypeText(fhkit)
	Else
		loWord.Selection.TypeText("     ")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("ba2") = .T.
	loWord.ActiveDocument.Bookmarks("ba2").Select
	If !EMPTY(fhbase)
		loWord.Selection.TypeText(fhbase)
	Else		
		loWord.Selection.TypeText("     ")
	EndIf	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("tm2") = .T.
	loWord.ActiveDocument.Bookmarks("tm2").Select
	If !EMPTY(multitemp)
		loWord.Selection.TypeText(multitemp)
	Else
		loWord.Selection.TypeText("___")
	EndIf		
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_ser_no_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_ser_no_3").Select
	loWord.Selection.TypeText(serial_no)	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_mod_no_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_mod_no_3").Select
	loWord.Selection.TypeText(model_no)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_company_name_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_company_name_3").Select
	loWord.Selection.TypeText(company_name)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_ordno_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_ordno_3").Select
	loWord.Selection.TypeText(orderno)	
ENDIF

If loWord.ActiveDocument.Bookmarks.Exists("v_coid_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid_3").Select
	loWord.Selection.TypeText(orderno)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_st_3") = .T.
	loWord.ActiveDocument.Bookmarks("v_st_3").Select
	If !EMPTY(state) 
		loWord.Selection.TypeText(state)
	Else
		loWord.Selection.TypeText("       ")
	EndIf		
ENDIF

If loWord.ActiveDocument.Bookmarks.Exists("v_ser_no_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_ser_no_4").Select
	loWord.Selection.TypeText(serial_no)	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_mod_no_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_mod_no_4").Select
	loWord.Selection.TypeText(model_no)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_company_name_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_company_name_4").Select
	loWord.Selection.TypeText(company_name)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_ordno_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_ordno_4").Select
	loWord.Selection.TypeText(orderno)	
EndIf

If loWord.ActiveDocument.Bookmarks.Exists("v_st_4") = .T.
	loWord.ActiveDocument.Bookmarks("v_st_4").Select
	If !EMPTY(state) 
		loWord.Selection.TypeText(state)
	Else
		loWord.Selection.TypeText("       ")
	EndIf		
ENDIF

If loWord.ActiveDocument.Bookmarks.Exists("v_po_no_1") = .T.
	loWord.ActiveDocument.Bookmarks("v_po_no_1").Select
	If Empty(pono)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_________________ ")
	Else
		loWord.Selection.TypeText(trim(pono))	
	EndIf		
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_po_no_2") = .T.
	loWord.ActiveDocument.Bookmarks("v_po_no_2").Select
	If Empty(pono)
	    loWord.Selection.Font.Underline = 0   
		loWord.Selection.TypeText("_________________ ")
	Else
		loWord.Selection.TypeText(trim(pono))	
	EndIf		
EndIf	


loWord.ActiveDocument.PrintOut()
WAIT WINDOW  "Printing Word Document: "+ALLTRIM(DOC) TIMEOUT 4
loWord.Application.Quit(.F.)


