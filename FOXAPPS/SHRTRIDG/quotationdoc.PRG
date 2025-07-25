   *******************************************************
   * PROGRAM NAME: QUOTATIONDOC.PRG                        
   *-----------------------------------------------------
   * This routine opens a WORD doc called QUOTATION FORM	 
   * and populates fields  	
   *******************************************************

*--Word macro modified for Visual FoxPro
PARAMETERS DOC, QUOTENO, TYPE, PRINT

#DEFINE True .T.
#DEFINE False .F.

LOCAL loWord

PATH = ""
DO shrtridg\QUOTATIONPATH

loWord = CreateObject("word.application")

loWord.Documents.Open (PATH+DOC, .F., .T.)
loWord.Visible = .F.
loWord.Visible = .T.


coid = " "
coname = " "
orderno = " "
serialno = " "
faxarea = " "
faxphone = " "
areacode = " "
phoneno = " "
ext = " "
firstname = " "
lastname = " "
shipvia = " "
quotedate = " "
customnotes = " "
ccomments = " "
cfrom = " "
cemail = " "
apprxship = " "
ref_no = " "
name = " "
freight = 0
taxes = 0
TOTAL = 0
quantity = 0
partno = " "
cost = 0
description = " "
totalext = 0
cNotes = " "



SELECT QUOTE_HEADER
LOCATE FOR ALLTRIM(quote_no) == ALLTRIM(quoteno) AND NOT DELETED()

STORE co_id to coid
STORE co_name to coname
STORE order_no to orderno
STORE serial_no TO serialno
STORE fax_arcode TO faxarea
STORE fax_phone TO faxphone
STORE area_code TO areacode
STORE phone_no TO phoneno
STORE frst_name TO firstname
STORE last_name TO lastname
STORE ship_via TO shipvia
STORE quote_date TO quotedate
STORE cust_notes TO customnotes
STORE comments TO ccomments
STORE from TO cFrom
STORE apprx_ship TO apprxship
STORE ref_no TO refno
STORE freight TO freight
STORE total TO total
STORE taxes TO taxes 
STORE notes TO cnotes
STORE email TO cemail
STORE sub_total TO subtotal
STORE ref_no TO refno
STORE extension TO ext

phoneno = "(" + ALLTRIM(areacode) + ") " + ALLTRIM(phoneno)
faxphone = "(" + ALLTRIM(faxarea) + ") " + ALLTRIM(faxphone)
name = ALLTRIM(firstname) + " " + ALLTRIM(lastname)

IF EMPTY(apprxship)
	approxship = "Approximate Shipping Date ____________, F.O.B. our plant, Scottsdale, AZ"
ELSE 
	approxship = "Approximate Shipping Date " + apprxship + ", F.O.B. our plant, Scottsdale, AZ"	
ENDIF	

IF EMPTY(CCOMMENTS)
	CCOMMENTS = "  "
ENDIF

IF EMPTY(CUSTOMNOTES)
	CUSTOMNOTES = "  "
ENDIF 

IF EMPTY(cNOTES)
	cNOTES = "          "
ENDIF

If loWord.ActiveDocument.Bookmarks.Exists("v_coid") = .T.
	loWord.ActiveDocument.Bookmarks("v_coid").Select
	loWord.Selection.TypeText(TRIM(coid))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_coname") = .T.
	loWord.ActiveDocument.Bookmarks("v_coname").Select
	loWord.Selection.TypeText(trim(coname))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_quoteno") = .T.
	loWord.ActiveDocument.Bookmarks("v_quoteno").Select
	loWord.Selection.TypeText(trim(quoteno))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_orderno") = .T.
	loWord.ActiveDocument.Bookmarks("v_orderno").Select
	loWord.Selection.TypeText((orderno))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_email") = .T.
	loWord.ActiveDocument.Bookmarks("v_email").Select
	loWord.Selection.TypeText((cemail))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_refno") = .T.
	loWord.ActiveDocument.Bookmarks("v_refno").Select
	loWord.Selection.TypeText((refno))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_serialno") = .T.
	loWord.ActiveDocument.Bookmarks("v_serialno").Select
	loWord.Selection.TypeText((serialno))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_phoneno") = .T.
	loWord.ActiveDocument.Bookmarks("v_phoneno").Select
	loWord.Selection.TypeText(trim(phoneno))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_extension") = .T.
	loWord.ActiveDocument.Bookmarks("v_extension").Select
	loWord.Selection.TypeText("Ext. " + trim(ext))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_faxphone") = .T.
	loWord.ActiveDocument.Bookmarks("v_faxphone").Select
	loWord.Selection.TypeText(trim(faxphone))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_attention") = .T.
	loWord.ActiveDocument.Bookmarks("v_attention").Select
	loWord.Selection.TypeText((name))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_shipvia") = .T.
	loWord.ActiveDocument.Bookmarks("v_shipvia").Select
	loWord.Selection.TypeText((shipvia))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_quotedate") = .T.
	loWord.ActiveDocument.Bookmarks("v_quotedate").Select
	loWord.Selection.TypeText(DTOC(quotedate))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_customnotes") = .T.
	loWord.ActiveDocument.Bookmarks("v_customnotes").Select
	loWord.Selection.TypeText((customnotes))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_comments") = .T.
	loWord.ActiveDocument.Bookmarks("v_comments").Select
	loWord.Selection.TypeText((ccomments))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_from") = .T.
	loWord.ActiveDocument.Bookmarks("v_from").Select
	loWord.Selection.TypeText((cFrom))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_apprxship") = .T.
	loWord.ActiveDocument.Bookmarks("v_apprxship").Select
	loWord.Selection.TypeText((approxship))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_freight") = .T.
	loWord.ActiveDocument.Bookmarks("v_freight").Select
	loWord.Selection.TypeText(STR(freight, 10, 2))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_taxes") = .T.
	loWord.ActiveDocument.Bookmarks("v_taxes").Select
	loWord.Selection.TypeText(str(taxes, 10, 2))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_subtotal") = .T.
	loWord.ActiveDocument.Bookmarks("v_subtotal").Select
	loWord.Selection.TypeText(str(subtotal, 10, 2))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_total") = .T.
	loWord.ActiveDocument.Bookmarks("v_total").Select
	loWord.Selection.TypeText(str(total, 10, 2))	
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_notes") = .T.
	loWord.ActiveDocument.Bookmarks("v_notes").Select
	loWord.Selection.TypeText((cNotes))
EndIf	

If loWord.ActiveDocument.Bookmarks.Exists("v_refno") = .T.
	loWord.ActiveDocument.Bookmarks("v_refno").Select
	loWord.Selection.TypeText((refno))
EndIf	


N = 0
SELECT quote_detail
SET ORDER TO QUOTENTRY   && QUOTE_NO+STR(ENTRY_NO)
SCAN FOR ALLTRIM(quote_no) == ALLTRIM(quoteno) AND NOT DELETED()
	N = N + 1
ENDSCAN

IF TYPE = "QUOTE" AND N > 16
	IF N <= 31
		loWord.ActiveDocument.Bookmarks("v_qty").Select
		loWord.Selection.MoveDown(5,1)
		loWord.Selection.InsertRowsAbove(15)
		loWord.Selection.MoveUp(5, 1)
	ELSE
		loWord.ActiveDocument.Bookmarks("v_qty").Select
		loWord.Selection.MoveDown(5,1)
		loWord.Selection.InsertRowsAbove(N - 16)
		loWord.Selection.MoveUp(5, 1)	
	ENDIF	
ENDIF	
	
IF TYPE = "CREDIT" AND N > 12
	IF N <= 20
		loWord.ActiveDocument.Bookmarks("v_qty").Select
		loWord.Selection.MoveDown(5,1)
		loWord.Selection.InsertRowsAbove(8)
		loWord.Selection.MoveUp(5, 1)
	ELSE	
		loWord.ActiveDocument.Bookmarks("v_qty").Select
		loWord.Selection.MoveDown(5,1)
		loWord.Selection.InsertRowsAbove(N - 12)
		loWord.Selection.MoveUp(5, 1)	
	ENDIF	
ENDIF	

N = 1
SELECT quote_detail
SCAN FOR ALLTRIM(quote_no) == ALLTRIM(quoteno) AND NOT DELETED()
	STORE quant TO quantity
	STORE part TO partno
	STORE descrption TO description
	STORE cost TO price
	STORE total_cost TO totalext
	
	IF N = 1 
		If loWord.ActiveDocument.Bookmarks.Exists("v_qty") = .T.
			loWord.ActiveDocument.Bookmarks("v_qty").Select
			loWord.Selection.TypeText(str(quantity, 10, 2))	
		ENDIF
		
		If loWord.ActiveDocument.Bookmarks.Exists("v_partno") = .T.
			loWord.ActiveDocument.Bookmarks("v_partno").Select
			loWord.Selection.TypeText((partno))	
		EndIf	
		
		If loWord.ActiveDocument.Bookmarks.Exists("v_description") = .T.
			loWord.ActiveDocument.Bookmarks("v_description").Select
			loWord.Selection.TypeText(description)	
		EndIf	
		
		If loWord.ActiveDocument.Bookmarks.Exists("v_price") = .T.
			loWord.ActiveDocument.Bookmarks("v_price").Select
			loWord.Selection.TypeText(str(price, 10, 2))	
		EndIf	
		
		If loWord.ActiveDocument.Bookmarks.Exists("v_totalext") = .T.
			loWord.ActiveDocument.Bookmarks("v_totalext").Select
			loWord.Selection.TypeText(str(totalext, 10, 2))	
		EndIf	

	ELSE
		loWord.Selection.MoveRight(12)
		loWord.Selection.TypeText(str(quantity, 10, 2))	
		loWord.Selection.MoveRight(12)
		loWord.Selection.TypeText((partno))	
		loWord.Selection.MoveRight(12)
		loWord.Selection.TypeText(description)	
		loWord.Selection.MoveRight(12)
		loWord.Selection.TypeText(str(price, 10, 2))
		loWord.Selection.MoveRight(12)
		loWord.Selection.TypeText(str(totalext, 10, 2))			
	ENDIF
	
	N = N + 1
		
ENDSCAN

IF Print = "FALSE"
	IF TYPE = "QUOTE" 
		loWord.ActiveDocument.SaveAs(PATH +"QUOTE" + ALLTRIM(quoteno) + ".doc")
	ELSE
		loWord.ActiveDocument.SaveAs(PATH+"CREDIT" + ALLTRIM(quoteno) + ".doc")
	ENDIF	
	loWord.Application.Quit(.F.)
ELSE
   loWord.PrintOut()	
   WAIT WINDOW  "Printing Word Document" TIMEOUT 10  
   loWord.Application.Quit(.F.)
ENDIF	


