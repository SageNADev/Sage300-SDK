/*
* Kendo UI Localization Project for v2012.3.1114
* Copyright 2012 Telerik AD. All rights reserved.
*
* Français Canada (fr-CA) Language Pack
*
* Project home : https://github.com/loudenvier/kendo-global
* Kendo UI home : http://kendoui.com
* Author : Martin Auclair (Oaklair)
*
* This project is released to the public domain, although one must abide to the
* licensing terms set forth by Telerik to use Kendo UI, as shown bellow.
*
* Telerik's original licensing terms:
* -----------------------------------
* Kendo UI Web commercial licenses may be obtained at
* https://www.kendoui.com/purchase/license-agreement/kendo-ui-web-commercial.aspx
* If you do not own a commercial license, this file shall be governed by the
* GNU General Public License (GPL) version 3.
* For GPL requirements, please review: http://www.gnu.org/copyleft/gpl.html
*/

kendo.ui.Locale = "Français Canada (fr-CA)";
kendo.ui.ColumnMenu.prototype.options.messages =
  $.extend(kendo.ui.ColumnMenu.prototype.options.messages, {

      /* COLUMN MENU MESSAGES
      ****************************************************************************/
      sortAscending: "Tri croissant",
      sortDescending: "Tri décroissant",
      filter: "Filtrer",
      columns: "Colonnes"
      /***************************************************************************/
  });

kendo.ui.Groupable.prototype.options.messages =
  $.extend(kendo.ui.Groupable.prototype.options.messages, {

      /* GRID GROUP PANEL MESSAGES
      ****************************************************************************/
      empty: "Faites glisser un en-tête de colonne et déposez-le ici pour grouper par cette colonne"
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.messages =
  $.extend(kendo.ui.FilterMenu.prototype.options.messages, {

      /* FILTER MENU MESSAGES
      ***************************************************************************/
      info: "Afficher les éléments dont la valeur :", // sets the text on top of the filter menu
      isTrue: "est vraie", // sets the text for "isTrue" radio button
      isFalse: "est fausse", // sets the text for "isFalse" radio button
      filter: "Filtrer", // sets the text for the "Filter" button
      clear: "Effacer", // sets the text for the "Clear" button
      and: "Et",
      or: "Ou",
      selectValue: "-Sélectionner une valeur-"
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.operators =
  $.extend(kendo.ui.FilterMenu.prototype.options.operators, {

      /* FILTER MENU OPERATORS (for each supported data type)
      ****************************************************************************/
      string: {
          eq: "Est égal à",
          neq: "N'est pas égal à",
          startswith: "Commence par",
          contains: "Contient",
          doesnotcontain: "Ne contient pas",
          endswith: "Se termine par"
      },
      number: {
          eq: "Est égal à",
          neq: "N'est pas égal à",
          gte: "Est supérieur ou égal à",
          gt: "Est supérieur à",
          lte: "Est inférieur ou égal à",
          lt: "Est inférieur à"
      },
      date: {
          eq: "Est égale à",
          neq: "N'est pas égale à",
          gte: "Est après ou égale à",
          gt: "Est après",
          lte: "Est avant ou égale à",
          lt: "Est avant"
      },
      enums: {
          eq: "Est égal à",
          neq: "N'est pas égal à"
      }
      /***************************************************************************/
  });

kendo.ui.Pager.prototype.options.messages =
  $.extend(kendo.ui.Pager.prototype.options.messages, {

      /* PAGER MESSAGES 
       ****************************************************************************/
      display: "{0} - {1} de {2} éléments",
      empty: "Aucun élément à afficher",
      page: "Page",
      of: "de {0}",
      itemsPerPage: "éléments par page",
      first: "Aller à la première page",
      previous: "Aller à la page précédente",
      next: "Aller à la page suivante",
      last: "Aller à la dernière page",
      refresh: "Actualiser"
      /***************************************************************************/
  });

kendo.ui.Validator.prototype.options.messages =
  $.extend(kendo.ui.Validator.prototype.options.messages, {

      /* VALIDATOR MESSAGES
      ****************************************************************************/
      required: "{0} est obligatoire.",
      pattern: "{0} n'est pas valide.",
      min: "{0} doit être supérieur ou égal à {1}.",
      max: "{0} doit être inférieur ou égal à {1}.",
      step: "{0} n'est pas valide.",
      email: "{0} n'est pas un courriel valide.",
      url: "{0} n'est pas une adresse URL valide.",
      date: "{0} n'est pas une date valide."
      /***************************************************************************/
  });

kendo.ui.ImageBrowser.prototype.options.messages =
  $.extend(kendo.ui.ImageBrowser.prototype.options.messages, {

      /* IMAGE BROWSER MESSAGES
      ****************************************************************************/
      uploadFile: "Téléverser",
      orderBy: "Arranger par",
      orderByName: "Nom",
      orderBySize: "Taille",
      directoryNotFound: "Aucun répertoire portant ce nom n'a été trouvé.",
      emptyFolder: "Répertoire vide",
      deleteFile: 'Êtes-vous sûr de vouloir supprimer "{0}"?',
      invalidFileType: "Le fichier sélectionné \"{0}\" n'est pas valide. Les types de fichiers valides sont {1}.",
      overwriteFile: "Un fichier portant le nom \"{0}\" est déjà présent dans le répertoire courant. Voulez-vous le remplacer?",
      dropFilesHere: "Déposer des fichiers à téléverser ici"
      /***************************************************************************/
  });

kendo.ui.Editor.prototype.options.messages =
  $.extend(kendo.ui.Editor.prototype.options.messages, {

      /* EDITOR MESSAGES
      ****************************************************************************/
      bold: "Gras",
      italic: "Italique",
      underline: "Souligné",
      strikethrough: "Barré",
      superscript: "Exposant",
      subscript: "Indice",
      justifyCenter: "Centrer",
      justifyLeft: "Aligner le texte à gauche",
      justifyRight: "Aligner le texte à droite",
      justifyFull: "Justifier",
      insertUnorderedList: "Insérer une liste non ordonnée",
      insertOrderedList: "Insérer une liste ordonnée",
      indent: "Augmenter le retrait",
      outdent: "Diminuer le retrait",
      createLink: "Insérer un lien hypertexte",
      unlink: "Supprimer un lien hypertexte",
      insertImage: "Insérer une image",
      insertHtml: "Insérer du HTML",
      fontName: "Sélectionner la famille de police",
      fontNameInherit: "(police héritée)",
      fontSize: "Sélectionner la taille de police",
      fontSizeInherit: "(taille héritée)",
      formatBlock: "Formatter",
      foreColor: "Couleur",
      backColor: "Couleur de fond",
      style: "Styles",
      emptyFolder: "Répertoire vide",
      uploadFile: "Téléverser",
      orderBy: "Arranger par:",
      orderBySize: "Taille",
      orderByName: "Nom",
      invalidFileType: "Le fichier sélectionné \"{0}\" n'est pas valide. Les types de fichiers valides sont {1}.",
      deleteFile: 'Êtes-vous sûr de vouloir supprimer "{0}"?',
      overwriteFile: 'Un fichier portant le nom "{0}" est déjà présent dans le répertoire courant. Voulez-vous le remplacer?',
      directoryNotFound: "Aucun répertoire portant ce nom n'a été trouvé.",
      imageWebAddress: "Adresse Web",
      imageAltText: "Texte alternatif",
      dialogInsert: "Insérer",
      dialogButtonSeparator: "ou",
      dialogCancel: "Annuler"
      /***************************************************************************/
  });