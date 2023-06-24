var sourcePosition = null;

document.addEventListener('DOMContentLoaded', function () {
    generateChessboard();
    fetchInitialState();

});

function generateChessboard() {
    var container = document.getElementById('chessboard-container');
    var isLightSquare = true;

    for (var row = 0; row < 8; row++) {
        var rowDiv = document.createElement('div');
        rowDiv.classList.add('row');

        for (var col = 0; col < 8; col++) {
            var square = document.createElement('div');
            square.classList.add('square');
            square.dataset.row = row;
            square.dataset.col = col;
            square.classList.add(isLightSquare ? 'light' : 'dark');
            square.addEventListener('click', squareClicked.bind(null, row, col));
            rowDiv.appendChild(square);
            isLightSquare = !isLightSquare;
        }

        container.appendChild(rowDiv);
        isLightSquare = !isLightSquare;
    }
}



function fetchInitialState() {
    $.ajax({
        url: '/Chess/InitialBoardState', // Change the URL to the correct endpoint
        type: 'GET',
        success: function (response) {
            console.log("Response from server:", response); // Log the response
            updateBoard(response.boardState);
        },
        error: function () {
            alert('Failed to fetch the initial state');
        }
    });
}



function squareClicked(row, col) {
    console.log(`Clicked square at row: ${row}, col: ${col}`);
    var square = document.querySelector(`.square[data-row='${row}'][data-col='${col}']`);
    var piece = square.querySelector('img');
    var pieceName = piece ? piece.alt : 'No piece';  // Assuming you have alt attributes for your images indicating piece names

    var selectedSquare = document.getElementById('selected-square');
    var selectedPiece = document.getElementById('selected-piece');

    selectedSquare.textContent = `Selected square: (${row}, ${col})`;
    selectedPiece.textContent = `Piece on selected square: ${pieceName}`;
    if (sourcePosition == null) {
        sourcePosition = { row: row, col: col };
    } else {
        var targetPosition = { row: row, col: col };

        $.ajax({
            url: '/Chess/MovePiece', // Adjust this to the correct route
            data: {
                sourceRow: sourcePosition.row,
                sourceColumn: sourcePosition.col,
                targetRow: targetPosition.row,
                targetColumn: targetPosition.col
            },
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    // Update the board state after a move
                    updateBoard(response.boardState);
                } else {
                    alert(response.responseText);
                }
                sourcePosition = null;
            },
            error: function () {
                alert("Error in ajax request.");
                sourcePosition = null;
            }
        });
    }
}

function updateBoard(boardState) {
    console.log("Received boardState:", boardState);

    if (!boardState || boardState.length !== 8) {
        console.error("Invalid boardState received:", boardState);
        return;
    }

    for (var row = 0; row < 8; row++) {
        for (var col = 0; col < 8; col++) {
            var square = document.querySelector(`.square[data-row='${row}'][data-col='${col}']`);

            if (!square) {
                console.error(`Square element not found for row=${row}, col=${col}`);
                continue;
            }

            var piece = boardState[row][col];
            var img = square.querySelector('img');
            
            console.log(`Processing square at row=${row}, col=${col}, piece=`, piece);

            if (piece && piece.imagePath) {
                var imgSrc = '/img/' + piece.imagePath; // Adjust this to the correct path
                console.log(`Piece found at row=${row}, col=${col}. Setting image source to ${imgSrc}`);

                if (img) {
                    img.src = imgSrc;
                } else {
                    var newImg = document.createElement('img');
                    newImg.src = imgSrc;
                    square.appendChild(newImg);
                    console.log(`New image element created for piece at row=${row}, col=${col}`);
                }
            } else {
                console.log(`No piece found at row=${row}, col=${col}`);
                if (img) {
                    square.removeChild(img);
                    console.log(`Removed image from square at row=${row}, col=${col}`);
                }
            }
        }
    }
}
