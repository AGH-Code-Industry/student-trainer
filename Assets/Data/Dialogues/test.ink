
NPC:Siema mordo, Właśnie zdałęm kolosa z Analizy. Mam nadzieję, że więcej tych hieroglifów nie zobaczę na oczy. Mam tak dobry dzień, że muszę się wypić za te wszytskie nieprzespane nocki. Idziesz ze mną?
NPC:to jak.

Hero: Muszę się zastanowić 

-> Wybor

=== Wybor ===
    #Ty
    NPC:Co wybierasz
    + Jasne.
        -> Alkoholizacja
    + [Nie dam rady.]
        -> Odpowiedzialnosc
    * Zastanowię się.
    -> Wybor



=== Alkoholizacja ===
    # Student Debil
    NPC:Chodź do Studenciaka. Muszę dziś pobić rekord w zerowaniu kuflowego mocnego.
    # Ty
    Hero:O ile dasz radę wyzerować szybciej ode mnie
    -> DONE

=== Odpowiedzialnosc ===
    # Ty
    Hero:Musze się nauczyć na jutrzejszego kolosa. Innym razem.
    -> DONE
    
END