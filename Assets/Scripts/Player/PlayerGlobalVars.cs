

public static class PlayerGlobalVars 
{
    private static string primaryGun, secondaryGun;
    


    public static string getPrimaryGun(){
        return primaryGun;
    }
    public static string getSecondaryGun(){
        return secondaryGun;
    }
    public static void setPrimaryGun(string name){
        primaryGun = name;
    }
    public static void setSecondaryGun(string name){
        secondaryGun = name;
    }
}