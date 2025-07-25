PARAMETERS lcQuoteNo,lcToWho, lcType

*--    Define our variables.
LOCAL MySession AS "outlook.application", ;
	  MyItem AS OBJECT, ;	
	  MyNameSpace AS OBJECT	
	  
PATH = ""
DO SHRTRIDG\QUOTATIONPATH.PRG

*--    Logon to MAPI and make the message.
MySession = CREATEOBJECT("outlook.application")
MyNameSpace = MySession.GetNamespace("MAPI")
MyItem = MySession.CreateItem(0)
MyItem.Subject = "Shortridge Instruments Quotation #: " + ALLTRIM(lcQuoteNo)
MyItem.Body = ""


IF lcTYPE = "BOTH"
	MyItem.Attachments.Add(PATH + "QUOTE" +ALLTRIM(lcquoteno)+ ".Doc", 1, 1, "QUOTE" + ALLTRIM(lcquoteno)+ ".Doc")	
	MyItem.Attachments.Add(PATH + "CREDIT" + ALLTRIM(lcquoteno)+ ".Doc", 1, 1, "CREDIT" + ALLTRIM(lcquoteno)+ ".Doc")	
ELSE
	MyItem.Attachments.Add(PATH+ALLTRIM(lcTYPE) + ALLTRIM(lcquoteno)+ ".Doc", 1, 1, lcTYPE + ALLTRIM(lcquoteno)+ ".Doc")		
ENDIF

MyItem.Recipients.ADD(lcToWho)
MyItem.Display	


SET DEFAULT TO (Path)
DO SETDEFAULTTO.PRG
SET DEFAULT TO (PATH)


