
var sourcePosition = null;
let turnDisplay;
let lastMove = null;
let sourcehighlight = null;
let BoardState;
let legalmoves;
let LegalMoves;

document.addEventListener('DOMContentLoaded', function () {
    turnDisplay = document.getElementById('turn-display')
    generateChessboard();
    fetchInitialState();

    // Add event listener for computer move button
    document.getElementById('computer-move-button').addEventListener('click', makeComputerMove);
});

function generateChessboard() {
    var container = document.getElementById('chessboard-container');
    var isLightSquare = true;

    for (var row = 7; row >= 0; row--) {
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
            document.getElementById('turn-display').innerText = "Current turn: " + response.currentTurn;
            document.getElementById('legal-moves').innerText = "# of legal moves: " + response.legalMoves;
            updateBoard(response.boardState);
        },
        error: function () {
            alert('Failed to fetch the initial state');
        }
    });
}

function makeComputerMove() {
    fetch('/Chess/MakeComputerMove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            updateBoard(data.boardState);
            document.getElementById('turn-display').innerText = "Current turn: " + data.currentTurn;
            document.getElementById('legal-moves').innerText = "# of legal moves: " + data.legalMoves;
        } else {
            alert(data.responseText);
        }
    })
    .catch(error => console.error('Error:', error));
}

function squareClicked(row, col) {
    console.log(`Clicked square at row: ${row}, col: ${col}`);
    var square = document.querySelector(`.square[data-row='${row}'][data-col='${col}']`);

    var selectedSquare = document.getElementById('selected-square');
    selectedSquare.textContent = `Selected square: (${row}, ${col})`;

    if (sourcePosition == null && BoardState[row][col] != null) {
        sourcePosition = { row: row, col: col };

        $.ajax({
            url: '/Chess/GetLegalMoves',
            data: { row: sourcePosition.row, col: sourcePosition.col },
            type: 'GET',
            success: function(response) {
                if (response.success) {
                    LegalMoves = response.LegalMoves;
                    console.log(`Legal moves recieved ` + LegalMoves);
                    highlightLegalMoves(LegalMoves);
                } else {
                    alert(response.responseText);
                }
            },
            error: function () {
                alert("Error in ajax request.");
            }
        });
    } else {
        var targetPosition = { row: row, col: col };

        $.ajax({
            url: '/Chess/MovePiece',
            data: {
                sourceRow: sourcePosition.row,
                sourceColumn: sourcePosition.col,
                targetRow: targetPosition.row,
                targetColumn: targetPosition.col,
            },
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    lastMove = [sourcePosition, targetPosition]; 
                    updateBoard(response.boardState);
                    document.getElementById('turn-display').innerText = "Current turn: " + response.currentTurn;
                    document.getElementById('legal-moves').innerText = "# of legal moves: " + response.legalMoves;
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

function highlightLegalMoves(moves) {
    moves.forEach(move => {
        var legalsquare = document.querySelector(`.square[data-row='${move.row}'][data-col='${move.col}']`);
        if(LegalMove) legalsquare.classList.add('legalsquares');
    })
}

function updateBoard(boardState) {
    console.log("Received boardState:", boardState);

    BoardState = boardState;

    if (!boardState || boardState.length !== 8) {
        console.error("Invalid boardState received:", boardState);
        return;
    }

    // Clear previous highlights
    document.querySelectorAll('.square').forEach(square => {
        square.classList.remove('highlight');
    });

    for (var row = 7; row >= 0; row--) {
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

    if (lastMove) {
        var sourceSquare = document.querySelector(`.square[data-row='${lastMove[0].row}'][data-col='${lastMove[0].col}']`);
        var targetSquare = document.querySelector(`.square[data-row='${lastMove[1].row}'][data-col='${lastMove[1].col}']`);

        if (sourceSquare) sourceSquare.classList.add('highlight');
        if (targetSquare) targetSquare.classList.add('highlight');
    }
}