using System;

namespace TSS.Core
{
    public readonly struct ApplicationQuitCancellationToken
    {
        private readonly Action _cancellationOperation;

        public ApplicationQuitCancellationToken(Action cancellationOperation) =>
            _cancellationOperation = cancellationOperation;

        public void Cancel() => _cancellationOperation();
    }
}