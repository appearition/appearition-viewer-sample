using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.Common
{
    /// <summary>
    /// Data container class for file transfer status when communicating with the EMS, for both download and upload.
    /// </summary>
    public class DataTransferStatus
    {
        #region Events 

        public delegate void ItemTransferComplete(DataTransferStatus status, ItemProgress item);

        /// <summary>
        /// Occurs when a single item transfer has completed.
        /// </summary>
        public event ItemTransferComplete OnItemTransferComplete;

        public delegate void DataTransferComplete(DataTransferStatus status);

        /// <summary>
        /// Occurs when all the items have been successfully transferred.
        /// </summary>
        public event DataTransferComplete OnDataTransferComplete;

        #endregion

        /// <summary>
        /// Information related to individual items being transferred.
        /// Do note that the transfers can either be linear (queue) or parallel.
        /// </summary>
        public class ItemProgress
        {
            public string fileName;
            public long fileSizeInBytes;
            float _progress;
            public bool isComplete = false;

            public float Progress
            {
                get => _progress;
                set => _progress = Mathf.Clamp01(value);
            }

            public long FileSizeInKb => fileSizeInBytes / 1000;
            public long FileSizeInMb => fileSizeInBytes / 1000000;

            public ItemProgress(string newFilename, long newFileSize)
            {
                fileName = newFilename;
                fileSizeInBytes = newFileSize;
            }
        }

        /// <summary>
        /// Contains transfer data related to each item being transferred, whereby the key is the item checksum, and the value is the item progress.
        /// </summary>
        public Dictionary<string, ItemProgress> itemProgress = new Dictionary<string, ItemProgress>();

        /// <summary>
        /// Whether or not this set of data transfer has completed.
        /// </summary>
        public bool IsDone { get; protected set; }

        /// <summary>
        /// Whether or not all the items to be tracked have been added to this transfer status.
        /// </summary>
        public bool IsLastItemEntered { get; protected set; }


        public DataTransferStatus()
        {
        }

        public DataTransferStatus(bool isInstant)
        {
            IsDone = isInstant;
            IsLastItemEntered = true;
        }

        #region Handy Properties 

        int? TotalAmountOfItemsOverride { get; set; }

        /// <summary>
        /// Total amount of items, including both the already completed and ongoing.
        /// </summary>
        public int TotalAmountOfItems => TotalAmountOfItemsOverride.HasValue ? TotalAmountOfItemsOverride.Value : itemProgress.Count;

        /// <summary>
        /// The amount of item whose transfer is complete.
        /// </summary>
        public int AmountOfItemsCompleted { get; protected set; }

        /// <summary>
        /// The amount of items which are still pending transfer, or are currently being transferred.
        /// </summary>
        public int AmountOfItemsRemaining => TotalAmountOfItems - AmountOfItemsCompleted;

        public float TotalSizeToTransferInBytes { get; protected set; }
        public float TotalSizeToTransferInKb => TotalSizeToTransferInBytes / 1000;
        public float TotalSizeToTransferInMb => TotalSizeToTransferInBytes / 1000000;


        /// <summary>
        /// The current progress based on all items' individual progress.
        /// </summary>
        public float CurrentProgressPerItem
        {
            get
            {
                if (IsDone)
                    return 1.0f;

                float accumulatedProgress = 0;
                foreach (var tmpItem in itemProgress.Values)
                    accumulatedProgress += tmpItem.Progress;

                return accumulatedProgress / TotalAmountOfItems;
            }
        }

        /// <summary>
        /// The current progress based on all items' progress using their transfer size.
        /// </summary>
        public float CurrentProgressPerSize
        {
            get
            {
                if (IsDone && IsLastItemEntered)
                    return 1.0f;

                float currentSize = 0;
                float maxSize = 0;

                foreach (var tmpItem in itemProgress.Values)
                {
                    maxSize += tmpItem.fileSizeInBytes;
                    currentSize += tmpItem.fileSizeInBytes * tmpItem.Progress;
                }

                if (Mathf.Approximately(maxSize, 0.0f))
                    return 0.0f;
                return currentSize / maxSize;
            }
        }

        #endregion

        #region Manage Content 

        /// <summary>
        /// Add a new item to the collection of item progresses.
        /// </summary>
        /// <param name="checksum"></param>
        /// <param name="filename"></param>
        /// <param name="sizeInBytes"></param>
        /// <param name="isLastItem"></param>
        public void AddNewItemProgress(string checksum, string filename, long sizeInBytes, bool isLastItem = false)
        {
            //Update storage
            itemProgress.Add(checksum, new ItemProgress(filename, sizeInBytes));
            TotalSizeToTransferInBytes += sizeInBytes;

            if (isLastItem)
                IsLastItemEntered = true;
        }

        /// <summary>
        /// Confirms that the last item to track has been entered.
        /// </summary>
        public void ConfirmLastItemEntered()
        {
            IsLastItemEntered = true;
        }

        /// <summary>
        /// Update the current progress value of a single item.
        /// </summary>
        /// <param name="checksum"></param>
        /// <param name="newProgress"></param>
        public void UpdateItemProgress(string checksum, float newProgress)
        {
            if (itemProgress.ContainsKey(checksum))
            {
                var tmpItem = itemProgress[checksum];
                float previousProgress = tmpItem.Progress;
                tmpItem.Progress = newProgress;

                if (!tmpItem.isComplete && previousProgress < 1.0f && tmpItem.Progress >= 1.0f)
                {
                    AmountOfItemsCompleted++;
                    tmpItem.isComplete = true;

                    if (OnItemTransferComplete != null)
                        OnItemTransferComplete(this, tmpItem);
                }
            }

            if (IsLastItemEntered && AmountOfItemsRemaining == 0)
            {
                IsDone = true;
                if (OnDataTransferComplete != null)
                    OnDataTransferComplete(this);
            }
        }

        /// <summary>
        /// Overrides the amount of items that will be transferred, without creating new entries.
        /// This is a bit of a hack to help estimations with linear download queues.
        /// </summary>
        /// <param name="amountOfItems"></param>
        public void SetTotalAmountOfItemsOverride(int amountOfItems)
        {
            TotalAmountOfItemsOverride = amountOfItems;
        }

        #endregion
    }
}