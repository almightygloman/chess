var sourcePosition = null;

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
            url: '@Url.Action("MovePiece", "Chess")', // Adjust this to the correct route
            data: {
                sourceRow: sourcePosition.row,
                sourceColumn: sourcePosition.col,
                targetRow: targetPosition.row,
                targetColumn: targetPosition.col
            },
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    if (sourcePosition.row == targetPosition.row && sourcePosition.col == targetPosition.col) {
                        // Display the legal moves for the piece
                        displayLegalMoves(response.legalMoves);
                    } else {
                        // Update the board state after a move
                        updateBoard(response.boardState);
                    }
                } else {
                    alert("Invalid move!");
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
    for (var row = 0; row < 8; row++) {
        for (var col = 0; col < 8; col++) {
            var square = document.querySelector(`.square[data-row='${row}'][data-col='${col}']`);
            var piece = boardState[row][col];
            var img = square.querySelector('img');

            if (piece) {
                var imgSrc = '/img/' + piece.ImagePath; // Adjust this to the correct path
                if (img) {
                    img.src = imgSrc;
                } else {
                    var newImg = document.createElement('img');
                    newImg.src = imgSrc;
                    square.appendChild(newImg);
                }
            } else {
                if (img) {
                    square.removeChild(img);
                }
            }
        }
    }
}
function displayLegalMoves(legalMoves) {
    // Remove the highlight from any previously highlighted squares
    var highlightedSquares = document.querySelectorAll('.highlight');
    highlightedSquares.forEach(function (square) {
        square.classList.remove('highlight');
    });

    // Add the highlight to the squares corresponding to the legal moves
    legalMoves.forEach(function (move) {
        var square = document.querySelector(`.square[data-row='${move.Row}'][data-col='${move.Column}']`);
        square.classList.add('highlight');
    });
}