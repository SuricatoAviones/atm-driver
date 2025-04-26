using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atm_driver.Clases
{
    public enum CodigoEvento
    {
        Comunicaciones = 1,
        ComunicacionesNoRegistrado = 2,
        BaseDeDatos = 3,
        ServidorCliente = 4,
        Download = 5
    }
    public enum EstadoCajero
    {
        Normal,
        InService,
        OutService,
        GetSupply,
        GetConfigurationInformation,
        Download
    }
    public enum EstadoCajeroEvento
    {
        EnServicio = 1,
        FueraDeServicio = 2,
        FueraDeLinea = 3,
        Inactivo = 4
    }
    public enum TipoCuenta
    {
        TARJETACREDITO = 07,
        OTRASCUENTAS = 08,
        AHORRO = 10,
        CORRIENTE = 20,
        CORRIENTE_CI = 21,
        LIBRETA_DORADA = 40
    }
    public enum TipoTransaccion
    {
        COMPRA_MAESTRO = 00,
        RETIRO = 01,
        PREAUTORIZACION = 04,
        AUTORIZACION = 05,
        CONFORMACION = 06,
        CONSULTA_MOVIMIENTOS = 07,
        CAMBIOCLAVE = 10,
        PAGO_TELEFONO = 11,
        CONSULTA = 30,
        TRANSFERENCIA = 40

    }

}
