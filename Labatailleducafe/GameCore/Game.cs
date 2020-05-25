using System;
using System.Net.Sockets;
using ServerCore;

namespace GameCore
{
    public class Game
    {
        private Server m_host;
        private Socket m_socket;
        private IA m_ia;
        private Player m_p1;
        private Player m_p2;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_host"></param>
        /// <param name="p_socket"></param>
        /// <param name="p_ia"></param>
        /// <param name="p_p1"></param>
        /// <param name="p_p2"></param>
        public Game(Server p_host, Socket p_socket, IA p_ia, ref Player p_p1, ref Player p_p2)
        {
            this.m_host = p_host;
            this.m_socket = p_socket;
            this.m_ia = p_ia;
            this.m_p1 = p_p1;
            this.m_p2 = p_p2;
        }

        /// <summary>
        /// Méthode permettant de jouer
        /// </summary>
        public void Play()
        {
            String v_continue = null;
            do
            {
                // Envoi du coup client (notre ia)
                this.m_host.SendData(m_socket, this.m_ia.Logic(ref m_p1));
                // Nous pouvons reçevoir ici la validation du coup ou non => VALI ou INVA
                String v_status = this.m_host.ReceiveData(m_socket, this.m_host.SizeResponse);
                // Nous pouvons reçevoir ici une coordonnée du coup serveur joué <lettre>:<number 0-9><number 0-9>
                String v_coord = this.m_host.ReceiveData(m_socket, this.m_host.SizeResponse);
                // Affecter la graine du joueur ennemi à l'ancienne graine
                byte v_x, v_y;
                if (v_coord != Response.FINI.ToString())
                {
                    try
                    {
                        v_x = Byte.Parse(v_coord[2].ToString());
                        v_y = Byte.Parse(v_coord[3].ToString());
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Error in a server reply");
                    }
                    if (v_status == Response.VALI.ToString() && this.m_ia.AllowToPlant(v_x, v_y))
                    {
                        this.m_ia.OldSeed.Zone = this.m_ia.Map.GetLetterParcel(v_x, v_y);
                        this.m_ia.OldSeed.X = v_x;
                        this.m_ia.OldSeed.Y = v_y;
                        this.m_ia.Map.ActualMap[v_x, v_y].Owner = m_p2.NumberOwner;
                        this.m_ia.Map.ActualMap[v_x, v_y].Available = false;
                        this.m_ia.CalculateOwner();
                    }
                }
                v_continue = this.m_host.ReceiveData(m_socket, this.m_host.SizeResponse);
            } while (Checker(v_continue, Response.ENCO));
            Score v_score = new Score(ref this.m_ia);
            v_score.ParcelScorePlayer(ref m_p1, ref m_p2);
            v_score.AdjacentScorePlayer(ref m_p1, ref m_p2);
        }

        /// <summary>
        /// Vérifier l'égalité entre une string et une response
        /// </summary>
        /// <param name="p_response"></param>
        /// <param name="p_toCompare"></param>
        /// <returns></returns>
        public Boolean Checker(String p_response, Response p_toCompare)
        {
            Boolean v_error = false;
            if (p_response.Equals(p_toCompare.ToString())){
                v_error = true;
            }
            return v_error;
        }

        /// <summary>
        /// Show winner
        /// </summary>
        public void ShowWinner()
        {
            string v_winner = (m_p1.Score.ParcelPoint + m_p1.Score.SeedAdjacent) > (m_p2.Score.ParcelPoint + m_p2.Score.SeedAdjacent) ? m_p1.Name :
            m_p2.Score.ParcelPoint + m_p2.Score.SeedAdjacent > m_p1.Score.ParcelPoint + m_p1.Score.SeedAdjacent ? m_p2.Name : "equality";
            Console.ForegroundColor = ConsoleColor.Cyan;
            if(v_winner != "equality")
                Console.WriteLine($"\nWinner is {v_winner} !");
            else
                Console.WriteLine($"\nOutch equality !");
            Console.ResetColor();
        }
    }
}
