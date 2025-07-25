*:*****************************************************************************
*:
*:        Program: C:\FOXAPPS\SHRTRIDG\ERR_FIX.PRG
*:         System: 
*:         Author: 
*:      Copyright (c) 1994, 
*:  Last modified: 02/24/94     15:48
*:
*:      Called by: STARTUP.PRG                       
*:          Calls: ERROR()            (function  in ?)
*:
*:      Documented 07/28/94 at 22:03               FoxDoc  version 2.10f
*:*****************************************************************************


DO FORM SHRTRIDG\ERROR WITH ERROR(), MESSAGE(), MESSAGE(1), LINENO()

*!*	CLOSE ALL
*!*	CLEAR PROGRAM
*!*	RELEASE ALL
*!*	CLEAR EVENTS

*!*	DEFINE WINDOW ERR_WIN FROM 10,10 TO 14,45 ;
*!*	   PANEL ;
*!*	   COLOR SCHEME 2

*!*	DO CASE   
*!*		CASE ERROR()=108
*!*		   ACTIVATE WINDOW ERR_WIN
*!*		   @ 1,0 SAY  "FILE IN USE BY ANOTHER."
*!*		   @ 2,0 SAY  ERROR()
*!*		   WAIT
*!*		   DEACTIVATE WINDOW ERR_WIN	   
*!*		   
*!*		CASE ERROR()=125
*!*		   ACTIVATE WINDOW ERR_WIN
*!*		   @ 1,0 SAY  "PRINTER NOT READY...."
*!*		   @ 2,0 SAY  ERROR()
*!*		   WAIT
*!*		   DEACTIVATE WINDOW ERR_WIN
*!*		   
*!*		CASE ERROR()=109
*!*		   ACTIVATE WINDOW ERR_WIN
*!*		   @ 1,0 SAY "RECORD IN USE BY ANOTHER."
*!*		   @ 2,0 SAY ERROR()
*!*		   WAIT
*!*		   DEACTIVATE WINDOW ERR_WIN	   
*!*		   
*!*		CASE ERROR()=43
*!*		   ACTIVATE WINDOW ERR_WIN
*!*		   @ 1,0 SAY "INSUFFICIENT MEMORY"
*!*		   @ 2,0 SAY ERROR()
*!*		   WAIT
*!*		   DEACTIVATE WINDOW ERR_WIN
*!*		   	   
*!*		OTHERWISE
*!*		   ACTIVATE WINDOW ERR_WIN
*!*		   @ 1,0 SAY "SEE SYSTEM ADMINISTRATOR"
*!*		   @ 2,0 SAY ERROR()
*!*		   WAIT
*!*		   DEACTIVATE WINDOW ERR_WIN   
*!*	ENDCASE

*!*	RETURN TO COMPMENU
*!*	*: EOF: ERR_FIX.PRG
