***************************************************
* PROGRAM NAME: TBSAVE.PRG                         *
*--------------------------------------------------*
* This routine saves the status of the system      *
* toolbars in the global array gaTbsets. Call the  *
* procedure TBREST to restore the system toolbars  *
* to their saved state                             *
****************************************************
PROCEDURE TBSAVE
  PUBLIC gaTbsets
  PRIVATE lnCnt,lnTB
  lnTB=11

  DIMENSION gaTbsets[lnTB,2]

  gaTbsets[1,1]="Color Palette"
  gaTbsets[2,1]="Database Designer"
  gaTbsets[3,1]="Form Controls"
  gaTbsets[4,1]="Form Designer"
  gaTbsets[5,1]="Layout"
  gaTbsets[6,1]="Print Preview"
  gaTbsets[7,1]="Query Designer"
  gaTbsets[8,1]="Report Controls"
  gaTbsets[9,1]="Report Designer"
  gaTbsets[10,1]="Standard"
  gaTbsets[11,1]="View Designer"

  FOR lnCnt=1 TO lnTB
    IF WEXIST(gaTbsets(lnCnt,1))
       gaTbsets(lnCnt,2)=.T.
       Hide Window (gaTbsets(lnCnt,1))
    ELSE
       gaTbsets(lnCnt,2)=.F.
    ENDIF
  ENDFOR
  RETURN
  
ENDPROC   