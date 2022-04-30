
// Variables
var map;
var infoWindow;


var markersData = [
   {
      lat: 32.5137913,
      lng: -116.886459,
      nome: "SUCURSAL CUCAPAH",
      morada1:"BLVD. CUCAPAH NO. 21352 COL. JARD&Iacute;N DORADO",
      morada2: "TIJUANA B.C.",
      codPostal: "TEL.(664) 103 55 83" 
   },
   {
      lat: 32.4973188,
      lng: -116.9330071,
      nome: "SUCURSAL PAPALOTE",
      morada1:"BLVD. INSURGENTES FRENTE A MACROPLAZA",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 625 12 52" 
   },
   {
      lat: 32.5090401,
      lng: -116.9873212,
      nome: "SUCURSAL PRADO",
      morada1:"BLVD. D&Iacute;AZ ORDAZ NO. 12678 COL. EL PRADO",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 608 01 64" 
   },
   {
      lat: 32.4919932,
      lng: -116.8515802,
      nome: "SUCURSAL PASEO 2000",
      morada1:"BLVD. 2000 TIJUANA,B.C.",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664)906 06 34" 
   },
   {
      lat: 32.4458417,
      lng: -117.0346438,
      nome: "SUCURSAL LA PAJARITA",
      morada1:"CALLE ORIENTE NO. 7002 LOMAS DEL MAR",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 900 10 54" 
   },
   {
      lat: 32.4651544,
      lng: -116.8644049,
      nome: "SUCURSAL LAS FUENTES",
      morada1:"BLVD. EL REFUGIO NO. 25420 COL. EL FLORIDO",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 676 37 15" 
   },
   {
      lat: 32.5236842,
      lng: -117.0337218,
      nome: "SUCURSAL MADERO",
      morada1:"AV. MADERO NO. 2070 ZONA CENTRO",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 634 04 31" 
   },
   {
      lat: 32.4960481,
      lng: -116.962143,
      nome: "SUCURSAL DIAZ ORDAZ",
      morada1:"BLVD. D&Iacute;AZ ORDAZ NO. 935 COL. SANTA FE",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 626 13 08" 
   },
   {
      lat: 32.5151263,
      lng: -117.0101628,
      nome: "SUCURSAL SALINAS",
      morada1:"BLVD. SALINAS ESQ. ESCUADR&Oacute;N 201 NO. 10513 COL. AVIACI&Oacute;N ",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL.(664) 686 28 79" 
   },
   {
      lat: 32.4810664,
      lng: -117.0358301,
      nome: "SUCURSAL FUNDADORES",
      morada1:"CENTRO COMERCIAL CALIMAX BLVD. FUNDADORES NO. 6802 FRACC. EL RUB&Iacute; ",
      morada2: "TIJUANA,B.C.",
      codPostal: "TEL. (664) 900 49 85" 
   },

	{
      lat: 22.901894,
      lng: -109.931388,
      nome: "SUCURSAL LOS CABOS",
      morada1:"CARRETERA TODO SANTOS KM 1.9 COL.INFONAVIT BRISAS DEL PACIFICO",
      morada2: "CABOS SAN LUCAS, B.C.S.",
      codPostal: "TEL. (624) 146 47 74" 
   }   
];


function initialize() {
   var mapOptions = {
      center: new google.maps.LatLng(32.5089973,-116.9873559),
      zoom: 9,
      mapTypeId: 'roadmap',
   };

   map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

   // crea una nueva Info Window con referencia al avariable windows
   // el contenido de sera atributo despues
   infoWindow = new google.maps.InfoWindow();

   // evento  infoWindow con click en mapa
   google.maps.event.addListener(map, 'click', function() {
      infoWindow.close();
   });

   // crear los marcadores para mostrar en el  mapa
   displayMarkers();
}
google.maps.event.addDomListener(window, 'load', initialize);



function displayMarkers(){

   // esta variable  va definir a área de mapa a desplegar el nível del zoom
   // de acuerdo con la pocion de los marcadores
   var bounds = new google.maps.LatLngBounds();
   
   // Loop que va estruturar la informacio  contenida em markersData
   // para que la funcion createMarker  crea los marcadores 
   for (var i = 0; i < markersData.length; i++){

      var latlng = new google.maps.LatLng(markersData[i].lat, markersData[i].lng);
      var nome = markersData[i].nome;
      var morada1 = markersData[i].morada1;
      var morada2 = markersData[i].morada2;
      var codPostal = markersData[i].codPostal;

      createMarker(latlng, nome, morada1, morada2, codPostal);

      // LOs valores de latitude y longitude del marcador son agregados
     
      bounds.extend(latlng);  
   }

   // Despues  de creados todos los marcadores
   // el API através da su funcion fitBounds va redefinir el nível del zoom
   // por consiguiente  área del mapa .
   map.fitBounds(bounds);
}

// Função que cria os marcadores e define o conteúdo de cada Info Window.
function createMarker(latlng, nome, morada1, morada2, codPostal){
   var marker = new google.maps.Marker({
      map: map,
      position: latlng,
      title: nome
   });
marker.setIcon('http://little-caesars.com.mx/images/LogoLCI3.png');

		
   // Evento que da instrucion al API para estar alerta del click del marcador.
   // Define o contenido  abre la Info Window.
   google.maps.event.addListener(marker, 'click', function() {
      
      // Variable que define la estrutura del HTML a insertar la Info Window.
      var iwContent = '<div id="iw_container">' +
            '<div class="iw_title">' + nome + '</div>' +
         '<div class="iw_content">' + morada1 + '<br />' +
         morada2 + '<br />' +
         codPostal + '</div></div>';
      
      // el contenido  de variable iwContent e inserto la Info Window.
      infoWindow.setContent(iwContent);

      // A Info Window esta aberta.
      infoWindow.open(map, marker);
   });
}