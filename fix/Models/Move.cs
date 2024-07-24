 public readonly struct Move
    {
        public readonly (int row, int col) SourcePosition;
        public readonly (int row, int col) TargetPosition;

        public Move((int Row, int Column) source, (int Row, int Col) target) : this()
        {
            this.SourcePosition = source;
            this.TargetPosition = target;
        }
    }