namespace xyzboutique.dataaccess.Core;
public class DataBaseQuery
{
 
  public static string ConValidadorDataExistencia = "uspValidadorDataExistencia"; //OK         
  public static string SegNombrePersonaPorTokenDevolver = "ufnNombrePersonaPorTokenDevolver";
  public static string SegIdUsuarioPorTokenDevolver = "ufnIdUsuarioPorTokenDevolver";
  public static string SegNombreUsuarioPorTokenDevolver = "ufnNombreUsuarioPorTokenDevolver";
  public static string SegUserSearchByUserName = "uspUserSearchByUserName";
  public static string SegUserHashByIdUser = "uspUserHashSearchByIdUser";
   public static string SecRoleSearch = "uspRoleSearch";
  public static string SegUserSearch = "uspUserSearch";
  public static string SegUserInd = "uspUserInd";
  public static string SegUsuarioHashEliminar = "uspUsuarioHashEliminar";
  public static string SegEsContraseniaExpiradaDevolver = "ufnEsContraseniaExpiradaDevolver"; // 
  public static string SegUsuarioHashBuscarPorIdTipo = "uspUsuarioHashHistorialBuscarPorIdTipo";

}