using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Web.Shared
{
	public class Menu
	{
		public int Orden { get; set; }
		public string Texto { get; set; }
		public string Enlace { get; set; }
		public string Imagen { get; set; }

		public Menu(int orden, string texto, string enlace, string imagen)
		{
			Orden = orden;
			Texto = texto;
			Enlace = enlace;
			Imagen = imagen;
		}

	}
}
