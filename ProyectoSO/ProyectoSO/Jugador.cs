using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSO
{
    public class Jugador
    {
        //  Nombre, personaje
        string personaje; //Fireboy, Watergirl, Rockboy, ___
        int numero;
        string username;
        int puntos = 0;

        // Posición
        int posicion_X;
        int posicion_Y;


        // Nombre animaciones
        string normal = ".gif";
        string cae = "_cae.gif";
        string corriendo_izq = "_corriendo_izq.gif";
        string corriendo_der = "_corriendo_der.gif";
        string derrotado = "_derrotado.gif";
        string salto = "_salto.png";
        string salto_h_izq = "_salto_h_izq.gif";
        string salto_h_der = "_salto_h_der.gif";
        string saludo = "_saludando.gif";

        // Instrucciones del PictureBox 
        int altura;
        int ancho;


        // Constructor del Personaje a través del PictureBox del mapa
        public Jugador(int num, string username, int pos_X, int pos_Y)
        {
            this.numero = num;
            this.username = username;
            UpdateNombrePersonaje(num);
            UpdateAnimaciones();
            this.posicion_X = pos_X;
            this.posicion_Y = pos_Y;

        }
        // Selección del personaje y animaciones
        public void UpdateNombrePersonaje(int num)
        {
            //string nombrePicturebox = (string)x.Name;
            switch (num)
            {
                case 1:
                    this.personaje = "Fireboy";
                    // Medidas picturebox
                    this.ancho = 60;
                    this.altura = 75;
                    break;
                case 2:
                    this.personaje = "Watergirl";
                    // Medidas picturebox
                    this.ancho = 70;
                    this.altura = 85;
                    break;
                case 3:
                    this.personaje = "Rockboy";
                    // Medidas picturebox
                    this.ancho = 60;
                    this.altura = 75;
                    break;
                case 4:
                    this.personaje = "Cloudgirl";
                    this.ancho = 70;
                    this.altura = 75;
                    break;
            }
        }
        public void UpdateAnimaciones()
        {
            this.normal = personaje + ".gif";
            this.cae = personaje + "_cae.gif";
            this.corriendo_izq = personaje + "_corriendo_izq.gif";
            this.corriendo_der = personaje + "_corriendo_der.gif";
            this.derrotado = personaje + "_derrotado.gif";
            this.salto = personaje + "_salto.png";
            this.salto_h_izq = personaje + "_salto_h_izq.gif";
            this.salto_h_der = personaje + "_salto_h_der.gif";
            this.saludo = personaje + "_saludando.gif";

        }

        // Aumenta puntos
        public void SumaPuntos()
        {
            this.puntos++;
        }
        // Getters
        public int GetPuntos()
        { return this.puntos; }
        public string GetUsername()
        { return this.username; }
        public string GetPersonajeNombre()
        { return this.personaje; }
        public int GetAnchoNormal()
        { return this.ancho; }
        public int GetAlturaNormal()
        { return this.altura; }

        public int GetX()
        { return this.posicion_X; }
        public int GetY()
        { return this.posicion_Y; }

        public string GetAnimacionNormal()
        { return this.normal; }
        public string GetAnimacionCae()
        { return this.cae; }
        public string GetAnimacionCorriendo_Der()
        { return this.corriendo_der; }
        public string GetAnimacionCorriendo_Izq()
        { return this.corriendo_izq; }
        public string GetAnimacionDerrotado()
        { return this.derrotado; }
        public string GetAnimacionSalto()
        { return this.salto; }
        public string GetAnimacionSalto_H_Der()
        { return this.salto_h_der; }
        public string GetAnimacionSalto_H_Izq()
        { return this.salto_h_izq; }
        public string GetAnimacionSaludo()
        { return this.saludo; }


        public void SetX(int x)
        { this.posicion_X = x; }
        public void SetY(int y)
        { this.posicion_Y = y; }
        // Movimiento
        public void MoverDerecha(int velocidad)
        { this.posicion_X = posicion_X + velocidad; }
        public void MoverIzquierda(int velocidad)
        { this.posicion_X = posicion_X - velocidad; }
        public void MoverVertical(int velocidad, int fuerza, bool saltando)
        {   // Mecanismo de salto + gravedad
            this.posicion_Y = posicion_Y + velocidad;

            if (saltando == true && fuerza < 0)
            {   // Cuando fuerza sea 0, hemos llegado a la maxima altura
                saltando = false;
            }
            if (saltando == true)
            {
                velocidad = -8;
                fuerza -= 1;
            }
            else
            { // gravedad
                velocidad = 10;
            }
        }
    }
}