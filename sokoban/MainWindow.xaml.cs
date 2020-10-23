﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sokoban
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Jeu jeu;

        public MainWindow()
        {
            InitializeComponent();
            jeu = new Jeu();

            this.KeyDown += MainWindow_KeyDown;
            Dessiner();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right)|| e.Key.Equals(Key.Left) || e.Key.Equals(Key.Down) || e.Key.Equals(Key.Up)) 
            {
                jeu.ToucheAppuyee(e.Key);
                Redessiner();

                if (jeu.Fini())
                {
                    MessageBoxResult msg = MessageBox.Show("Bravo, Vous avez gagné en " + jeu.NbDeplacements + " mouvements.\n\tVoulez-vous recommencer ? ","Recommencer? ", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (msg == MessageBoxResult.Yes)
                    {
                        jeu.Restart();
                        Redessiner();
                    }
                }
            }   
        }

        private void Redessiner()
        {
            cnvMobile.Children.Clear();
            DessinerCaisses();
            DessinerPersonnage();
            AfficherNbDeplacements();
        }

        private void AfficherNbDeplacements()
        {

            txtNbDeplacement.Text = jeu.NbDeplacements.ToString();
        }

        private void Dessiner()
        {
            DessinerCarte();
            Redessiner();
        }

        private void DessinerPersonnage()
        {
            Rectangle r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Margin = new Thickness(jeu.Personnage.y * 50  ,jeu.Personnage.x * 50 , 0, 0);
            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/sangoku.png", UriKind.Relative)));
            cnvMobile.Children.Add(r);
        }

        private void DessinerCaisses()
        {
            foreach (Position pos  in jeu.Caisses)
            {
                Rectangle r = new Rectangle();
                r.Width = 42;
                r.Height = 42;
                r.Margin = new Thickness(pos.y * 50+4, pos.x * 50+4, 0, 0);
                r.Fill = new ImageBrush(new BitmapImage(new Uri("images/caisse.png", UriKind.Relative)));
                cnvMobile.Children.Add(r);

            }        }

        private void DessinerCarte()
        {
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = 50;
                    r.Height = 50;
                    r.Margin = new Thickness(colonne * 50, ligne * 50, 0, 0);
                    switch (jeu.Case(ligne, colonne))
                    {
                        case Jeu.Etat.Vide:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/sol.png", UriKind.Relative)));
                            break;

                        case Jeu.Etat.Mur:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/mur.png", UriKind.Relative)));
                            break;

                        case Jeu.Etat.Cible:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/cible.png", UriKind.Relative)));
                            break;
                    }
                   
                    cnvGrille.Children.Add(r);
                }
            }
        }

        private void btnRecommencer_Click(object sender, EventArgs e) 
        {
           
            Redessiner();
            jeu.Restart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Redessiner();
            jeu.Restart();
        }
    }
}
