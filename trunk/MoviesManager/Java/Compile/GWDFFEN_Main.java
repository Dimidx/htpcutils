/**
 * Code généré par WinDev - NE PAS MODIFIER !
 * Objet WinDev : Fenêtre
 * Classe Java : FEN_Main
 * Date : 29/12/2008 15:51:41
 * Version de WDJAVA.DLL  : 14.00Cl
 */


import fr.pcsoft.wdjava.framework.*;
import fr.pcsoft.wdjava.framework.projet.*;
import fr.pcsoft.wdjava.framework.ihm.*;
import fr.pcsoft.wdjava.framework.ihm.menu.*;
import fr.pcsoft.wdjava.api.WDMAT.*;
import fr.pcsoft.wdjava.api.WDOBJ.*;
import fr.pcsoft.wdjava.api.WDSTD.*;
import fr.pcsoft.wdjava.api.WDVM.*;
import fr.pcsoft.wdjava.api.WDJava.*;
import fr.pcsoft.wdjava.framework.poo.*;
import fr.pcsoft.wdjava.api.WDHF.*;
import fr.pcsoft.wdjava.framework.hf.*;
import fr.pcsoft.wdjava.framework.indirection.*;
import fr.pcsoft.wdjava.api.WDCOM.*;
import fr.pcsoft.wdjava.framework.exception.*;
import fr.pcsoft.wdjava.api.WDPRN.*;
import fr.pcsoft.wdjava.ext.ui.*;
import fr.pcsoft.wdjava.api.WDZIP.*;
import fr.pcsoft.wdjava.framework.pourtout.*;
import fr.pcsoft.wdjava.api.WDXML.*;
import fr.pcsoft.wdjava.framework.ihm.action.*;



public class GWDFFEN_Main extends WDFenetre
{

////////////////////////////////////////////////////////////////////////////
// Déclaration des champs de FEN_Main
////////////////////////////////////////////////////////////////////////////

/**
 * STC_Slogan
 */
class GWDSTC_Slogan extends WDLibelle
{
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setNouvellePolice("Trebuchet MS",9.000000,true,false);
setQuid(2979369322915273421l);
setChecksum("1281915074");
setNom("STC_Slogan");
setType(3);
setBulle("");
setLibelle("Protecting the future");
setNote("");
setCurseurSouris(0);
setCouleur(0x2D2D2D);
setCouleurFond(0xFFFFFFFF);
setEtatInitial(0);
setXInitial(642);
setYInitial(30);
setLargeurInitiale(126);
setHauteurInitiale(18);
setPlan(0);
setCadrageHorizontal(2);
setCadrageVertical(1);
setAncrage(4);
setLargeurMin(0);
setHauteurMin(0);
setLargeurMax(134217727);
setHauteurMax(134217727);
setVisibleInitial(true);
setAltitude(1);
setTauxAncrageDroite(1000);
setTauxAncrageLargeur(1000);
setTauxAncrageBas(1000);
setTauxAncrageHauteur(1000);
setTypeAnimation(2);
setCouleurClignotement(-1);
setDureeAnimation(300);
setAnimationInitiale(false);
setPresenceLibelle(true);
setStyleLibelle(0x2D2D2D, "Trebuchet MS", -12.000000, false, true, 3);
setCadreExterieur(1, 0xFFFFFFFF, 0x696969, 0x2D2D2D, 4, 4);
activerEcoute();
}

// Activation des écouteurs: 
public void activerEcoute()
{
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDSTC_Slogan mWD_STC_Slogan;

/**
 * STC_CompanyName
 */
class GWDSTC_CompanyName extends WDLibelle
{
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setNouvellePolice("Segoe UI",14.250000,true,false);
setQuid(2979369322915338957l);
setChecksum("1281980610");
setNom("STC_CompanyName");
setType(3);
setBulle("");
setLibelle("Pure Company");
setNote("");
setCurseurSouris(0);
setCouleur(0x2D2D2D);
setCouleurFond(0xFFFFFFFF);
setEtatInitial(0);
setXInitial(621);
setYInitial(6);
setLargeurInitiale(165);
setHauteurInitiale(30);
setPlan(0);
setCadrageHorizontal(2);
setCadrageVertical(1);
setAncrage(4);
setLargeurMin(0);
setHauteurMin(0);
setLargeurMax(134217727);
setHauteurMax(134217727);
setVisibleInitial(true);
setAltitude(2);
setTauxAncrageDroite(1000);
setTauxAncrageLargeur(1000);
setTauxAncrageBas(1000);
setTauxAncrageHauteur(1000);
setTypeAnimation(2);
setCouleurClignotement(-1);
setDureeAnimation(300);
setAnimationInitiale(false);
setPresenceLibelle(true);
setStyleLibelle(0x2D2D2D, "Segoe UI", -19.000000, false, true, 3);
setCadreExterieur(1, 0xFFFFFFFF, 0x696969, 0x0, 4, 4);
activerEcoute();
}

// Activation des écouteurs: 
public void activerEcoute()
{
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDSTC_CompanyName mWD_STC_CompanyName;

/**
 * ONG_Onglet1
 */
class GWDONG_Onglet1 extends WDOnglet
{

////////////////////////////////////////////////////////////////////////////
// Déclaration des champs de FEN_Main.ONG_Onglet1
////////////////////////////////////////////////////////////////////////////

/**
 * ONG_Onglet1_Volet1
 */
////////////////////////////////////////////////////////////////////////////
// Déclaration des champs du volet n°1 de FEN_Main.ONG_Onglet1
////////////////////////////////////////////////////////////////////////////

/**
 * BTN_Bouton1
 */
class GWDBTN_Bouton1 extends WDBouton
{
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setNouvellePolice("Trebuchet MS",8.250000,false,false);
setQuid(2979374391358954588l);
setChecksum("1664188597");
setNom("BTN_Bouton1");
setType(4);
setBulle("");
setLibelle("&Bouton");
setMenuContextuelSysteme();
setNote("");
setCurseurSouris(0);
setCouleur(0xEBEBEB);
setCouleurFond(0xE8EBEE);
setNavigable(true);
setEtatInitial(0);
setXInitial(360);
setYInitial(170);
setLargeurInitiale(80);
setHauteurInitiale(24);
setPlan(0);
setClicDroit("");
setAncrage(0);
setImageEtat(1);
setImageFondEtat(5);
setLargeurMin(0);
setHauteurMin(0);
setLargeurMax(134217727);
setHauteurMax(134217727);
setVisibleInitial(true);
setNumTab(2);
setAltitude(1);
setTauxAncrageDroite(1000);
setTauxAncrageLargeur(1000);
setTauxAncrageBas(1000);
setTauxAncrageHauteur(1000);
setLettreAppel(65535);
setTypeBouton(0);
setTypeActionPredefinie(0);
setLargeurHalo(0);
setHauteurHalo(0);
setPresenceLibelle(true);
setImage("", 0, 1);
setLibelleVAlign(1);
setLibelleHAlign(1);
setStyleLibelleRepos(0xEBEBEB, "Trebuchet MS", -11.000000, false, false);
setStyleLibelleSurvol(0xEBEBEB, "Trebuchet MS", -11.000000, false, false);
setStyleLibelleEnfonce(0xEBEBEB, "Trebuchet MS", -11.000000, false, false);
setStyleCadreRepos(17, 0xE8EBEE, 0x696969, 0x0, 4, 4);
setStyleCadreSurvol(17, 0xE8EBEE, 0x696969, 0x0, 4, 4);
setStyleCadreEnfonce(17, 0xE8EBEE, 0x696969, 0x0, 4, 4);
setImageFondTroisImage(false);
setImageFond9Images(true);
setImageFond("D:\\Mes Projets\\MoviesManager\\Pure Automn_Btn_Std.png", 1, 0, 1, 6);
activerEcoute();
}

/**
 * Traitement: Clic sur BTN_Bouton1
 */
public void clicSurBoutonGauche()
{
super.clicSurBoutonGauche();


try
{
// XMLDocument("DocXML",fChargeTexte("D:\\Perso\\Films\\Le Cactus.nfo"))
WDAPIXml.xmlDocument("DocXML",WDAPIFichier.fChargeTexte("D:\\Perso\\Films\\Le Cactus.nfo"));

// Erreur sur l'objet: FEN_Main.ONG_Onglet1.BTN_Bouton1
// 	Traitement : Clic sur BTN_Bouton1
// 	Code: 1000012
// 	Message: La fonction <XMLLit> n'a pas d'équivalent dans le framework WL/Java.
// 	Ligne : 1, Colonne : 6
// 
// 
WDAPIJava.erreurGeneration("Erreur sur l'objet: FEN_Main.ONG_Onglet1.BTN_Bouton1\r\n\tTraitement : Clic sur BTN_Bouton1\r\n\tCode: 1000012\r\n\tMessage: La fonction <XMLLit> n'a pas d'équivalent dans le framework WL/Java.\r\n\tLigne : 1, Colonne : 6\r\n\r\n");


// // trace(xmllit("DocXML","/movie/title"))
// WDAPIDivers.trace(.getString());
// 

// // XMLTermine("DocXML")
// WDAPIXml.xmlTermine("DocXML");
// 
// // CAS ERREUR:
// }
// catch(WDErreurNonFatale e)
// {
// //Code du CAS ERREUR:
// 
// // 	XMLTermine("DocXML")
// WDAPIXml.xmlTermine("DocXML");
// 
// // 	erreur(ErreurInfo(errMessage))
// WDAPIDivers.erreur(WDAPIVM.erreurInfo(1).getString());
// 
}
}




// Activation des écouteurs: 
public void activerEcoute()
{
activerEcouteurClic();
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDBTN_Bouton1 mWD_BTN_Bouton1 = new GWDBTN_Bouton1();
class GWDONG_Onglet1_Volet1 extends WDVoletOnglet
{
/**
 * Initialise tous les champs de FEN_Main.ONG_Onglet1
 */
public void initialiserSousObjets()
{
////////////////////////////////////////////////////////////////////////////
// Initialisation des champs de FEN_Main.ONG_Onglet1
////////////////////////////////////////////////////////////////////////////
mWD_BTN_Bouton1.initialiserObjet();
ajouterFils("BTN_Bouton1", mWD_BTN_Bouton1);
}
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setOnglet(getWDOngletThis());
setQuid(2979373386119711261l);
setType(0);
setLibelle("Détails");
setEtatInitial(0);
setVisibleInitial(true);
setLettreAppel(0);
setImage("");
initialiserSousObjets();
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDONG_Onglet1_Volet1 mWD_ONG_Onglet1_Volet1 = new GWDONG_Onglet1_Volet1();

/**
 * ONG_Onglet1_Volet2
 */
////////////////////////////////////////////////////////////////////////////
// Déclaration des champs du volet n°2 de FEN_Main.ONG_Onglet1
////////////////////////////////////////////////////////////////////////////
class GWDONG_Onglet1_Volet2 extends WDVoletOnglet
{
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setOnglet(getWDOngletThis());
setQuid(2979373386119776797l);
setType(0);
setLibelle("Fanart");
setEtatInitial(0);
setVisibleInitial(true);
setLettreAppel(0);
setImage("");
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDONG_Onglet1_Volet2 mWD_ONG_Onglet1_Volet2 = new GWDONG_Onglet1_Volet2();
/**
 * Initialise tous les champs de FEN_Main.ONG_Onglet1
 */
public void initialiserSousObjets()
{
////////////////////////////////////////////////////////////////////////////
// Initialisation des champs de FEN_Main.ONG_Onglet1
////////////////////////////////////////////////////////////////////////////
mWD_ONG_Onglet1_Volet1.initialiserObjet();
ajouterVolet(mWD_ONG_Onglet1_Volet1);
mWD_ONG_Onglet1_Volet2.initialiserObjet();
ajouterVolet(mWD_ONG_Onglet1_Volet2);
}
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setQuid(2979373386119645725l);
setChecksum("1447232236");
setNom("ONG_Onglet1");
setType(16);
setBulle("");
setLibelle("&Onglet");
setMenuContextuelSysteme();
setNote("");
setCurseurSouris(0);
setCouleurFond(0xFFFFFFFF);
setNavigable(true);
setEtatInitial(0);
setXInitial(247);
setYInitial(105);
setLargeurInitiale(539);
setHauteurInitiale(455);
setValeurInitiale("1");
setPlan(0);
setClicDroit("");
setAncrage(10);
setLargeurMin(0);
setHauteurMin(0);
setLargeurMax(134217727);
setHauteurMax(134217727);
setVisibleInitial(true);
setNumTab(1);
setAltitude(3);
setTauxAncrageDroite(1000);
setTauxAncrageLargeur(1000);
setTauxAncrageBas(1000);
setTauxAncrageHauteur(1000);
setLettreAppel(65535);
setPersistant(true);
setPresenceLibelle(false);
setCadreExterieur(2, 0x2D2D2D, 0x696969, 0x0, 4, 4);
setStyleVoletActif(1, 0, 0x2D2D2D, 0xEBEBEB, "Trebuchet MS", -11.000000, false, false, 0);
setStyleVoletInactif(1, 0, 0x696969, 0xEBEBEB, "Trebuchet MS", -11.000000, false, false, 0);
setImage("D:\\Mes Projets\\MoviesManager\\Pure Automn_Tab_Top.gif", true);
setImageEtat(1);
activerEcoute();
initialiserSousObjets();
}

// Activation des écouteurs: 
public void activerEcoute()
{
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDONG_Onglet1 mWD_ONG_Onglet1;

/**
 * LIB_Libellé1
 */
class GWDLIB_Libelle1 extends WDLibelle
{
public void initialiserObjet()
{
super.initialiserObjet();
setFenetre( getWDFenetreThis() );
setNouvellePolice("Trebuchet MS",14.250000,false,true);
setQuid(2979373588015792753l);
setChecksum("1479910471");
setNom("LIB_Libellé1");
setType(3);
setBulle("");
setLibelle("Libellé");
setNote("");
setCurseurSouris(0);
setCouleur(0xEBEBEB);
setCouleurFond(0xFFFFFFFF);
setEtatInitial(0);
setXInitial(6);
setYInitial(72);
setLargeurInitiale(780);
setHauteurInitiale(24);
setPlan(0);
setCadrageHorizontal(0);
setCadrageVertical(0);
setAncrage(0);
setLargeurMin(0);
setHauteurMin(0);
setLargeurMax(134217727);
setHauteurMax(134217727);
setVisibleInitial(true);
setAltitude(4);
setTauxAncrageDroite(1000);
setTauxAncrageLargeur(1000);
setTauxAncrageBas(1000);
setTauxAncrageHauteur(1000);
setTypeAnimation(2);
setCouleurClignotement(-1);
setDureeAnimation(300);
setAnimationInitiale(false);
setPresenceLibelle(true);
setStyleLibelle(0xEBEBEB, "Trebuchet MS", -19.000000, true, false, 3);
setCadreExterieur(1, 0xFFFFFFFF, 0x696969, 0x0, 4, 4);
activerEcoute();
}

// Activation des écouteurs: 
public void activerEcoute()
{
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
}
public GWDLIB_Libelle1 mWD_LIB_Libelle1;

/**
 * Traitement: Déclarations globales de FEN_Main
 */
public void declarerGlobale(WDObjet[] WD_tabParam)
{
// gclMonFilm est un Movie
vWD_gclMonFilm = new GWDCMovie();
ajouterVariableGlobale("gclMonFilm",vWD_gclMonFilm);



}



// Activation des écouteurs: 
public void activerEcoute()
{
}

////////////////////////////////////////////////////////////////////////////
// Déclaration des variables globales
////////////////////////////////////////////////////////////////////////////
 public WDObjet vWD_gclMonFilm = null;
////////////////////////////////////////////////////////////////////////////
// Création des champs de la fenêtre FEN_Main
////////////////////////////////////////////////////////////////////////////
protected void creerChamps()
{
mWD_STC_Slogan = new GWDSTC_Slogan();
mWD_STC_CompanyName = new GWDSTC_CompanyName();
mWD_ONG_Onglet1 = new GWDONG_Onglet1();
mWD_LIB_Libelle1 = new GWDLIB_Libelle1();

}
////////////////////////////////////////////////////////////////////////////
// Initialisation de la fenêtre FEN_Main
////////////////////////////////////////////////////////////////////////////
public void initialiserObjet()
{
setQuid(2979369322915142349l);
setChecksum("1288582643");
setNom("FEN_Main");
setType(1);
setBulle("");
setMenuContextuelSysteme();
setNote("");
setCouleur(0x0);
setCouleurFond(0x333333);
setXInitial(0);
setYInitial(0);
setLargeurInitiale(800);
setHauteurInitiale(600);
setTitre("Movies Manager");
setClicDroit("");
setMode9Images(new int[] {1,4,1,4,0,4,1,4,1});
setMargeMode9Images(173, 167, 162, 142);
setLargeurMin(-1);
setHauteurMin(-1);
setLargeurMax(20000);
setHauteurMax(20000);
setBarreDeMessage(false);
setPositionFenetre(3);
setBarreDeTitre(true);
setTypeCadre(0);
setRedimensionnable(true);
setDeplacementParLeFond(false);
setMenuOfficeXP(true);
setThemeXP(false);
setPersistant(true);
setGFI(true);
setImageFond("D:\\Mes Projets\\MoviesManager\\Pure Automn_Bg.jpg", 1, 0, 1);
setPoigneeRedimensionnement("D:\\Mes Projets\\MoviesManager\\Pure Automn_Resize.gif");
setMargeZoneCliente(0, 0, 0, 0);

activerEcoute();

////////////////////////////////////////////////////////////////////////////
// Initialisation des champs de FEN_Main
////////////////////////////////////////////////////////////////////////////
mWD_STC_Slogan.initialiserObjet();
m_conteneur.ajouterFils("STC_Slogan", mWD_STC_Slogan);
mWD_STC_CompanyName.initialiserObjet();
m_conteneur.ajouterFils("STC_CompanyName", mWD_STC_CompanyName);
mWD_ONG_Onglet1.initialiserObjet();
m_conteneur.ajouterFils("ONG_Onglet1", mWD_ONG_Onglet1);
mWD_LIB_Libelle1.initialiserObjet();
m_conteneur.ajouterFils("LIB_Libellé1", mWD_LIB_Libelle1);

}
}
