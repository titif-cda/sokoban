using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace sokoban
{
    class Jeu
    {
        public enum Etat
        {
            Vide,
            Mur,
            Cible
        }
        private Etat[,] grille;

        private List<Position> caisses;
        public List<Position> Caisses { get => caisses; }
      
        private Position personnage;
        public Position Personnage { get => personnage;}

        private int nbDeplacements;
        public int NbDeplacements { get => nbDeplacements; }



        static String grilleTxt = "..XXXXXX..XXX.oo.XXXX..o..o..XX........XXXX....XXX..XX.CXX...XXXC.XXX..X.CP.X.X..X......X..XXXXXXXX.";

       

        public Jeu()
        {
            grille = new Etat[10, 10];
            InitialisationCarte();
        }

        private void InitialisationCarte()
        {
            //Creer liste vide de caisse
            caisses = new List<Position>();

            //Pour chaque case , initialise la bonne valeur
            //Ajouter les caisses si besoins
            //Détermine la position de départ du personnage
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    switch (grilleTxt[ligne*10+colonne])
                    {
                        case '.':
                            grille[ligne, colonne] = Etat.Vide;
                        break;

                        case 'X':
                            grille[ligne, colonne] = Etat.Mur;
                            break;

                        case 'o':
                            grille[ligne, colonne] = Etat.Cible;
                            break;

                        case 'C':
                            Caisses.Add(new Position(ligne, colonne));
                            grille[ligne, colonne] = Etat.Vide;
                            break;

                        case 'P':
                            personnage = new Position(ligne, colonne);
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                    }
                }
            }
        }

        internal bool Fini()
        {
            foreach (Position caisse in caisses)
            {
                if (grille[caisse.x,caisse.y] != Etat.Cible)
                {
                    return false;
                }
            }
            return true;
        }

        public void ToucheAppuyee(Key key)
        {
            Position newPos = new Position(personnage.x, personnage.y);

            CalculNewPos(newPos, key);
            if (CaseOk(newPos, key))
            {
                personnage = newPos;
                nbDeplacements++;
            }

        }

        private static void CalculNewPos(Position newPos, Key key )
        {
            switch (key)
            {
                case Key.Down:
                    newPos.x++;
                    break;
                case Key.Up:
                    newPos.x--;
                    break;
                case Key.Left:
                    newPos.y--;
                    break;
                case Key.Right:
                    newPos.y++;
                    break;
            }
        }

        private bool CaseOk(Position newPos, Key key)
        {
            // Présence d'un mur
            if (grille[newPos.x,newPos.y]==Etat.Mur)
            {
                return false;
            }
            // Présence d'une caisse
            Position caisse = CaisseInPos(newPos);
            if (CaisseInPos(newPos) != null)
            {
                //Déplacement possible?
                Position newPosCaisse = new Position(caisse.x, caisse.y);
                CalculNewPos(newPosCaisse, key);

                if (grille[newPosCaisse.x,newPosCaisse.y]==Etat.Mur)
                {
                    return false;
                }
                else if (CaisseInPos(newPosCaisse) != null)
                {
                    return false;
                }
                else
                {
                    caisse.x = newPosCaisse.x;
                    caisse.y = newPosCaisse.y;
                    return true;

                }

            }
            //Pas d'obstacle
            return true;

        }

       

        private Position CaisseInPos(Position newPos)
        {
            foreach (Position caisse in caisses)
            {
                if (caisse.x == newPos.x && caisse.y == newPos.y)
                {
                    return caisse;
                }
            }
            return null;
        }

        public Etat Case(int ligne, int colonne)
        {
            return grille[ligne,colonne];
        }
        public void Restart()
        {
          
            InitialisationCarte();
            nbDeplacements = 0;
            
        }
    }
}
